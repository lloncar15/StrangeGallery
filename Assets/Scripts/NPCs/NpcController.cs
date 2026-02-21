using UnityEngine;

public class NpcController : MonoBehaviour, IColorAffectable {
    [Header("Config")]
    [SerializeField] private NpcStateData stateData;

    [Header("References")]
    [SerializeField] private Animator animator;

    private INpcState _currentState;

    public NpcStateData StateData => stateData;
    public NpcHealthComponent Health { get; private set; }
    public NpcMovementComponent Movement { get; private set; }
    public PlayablePaintingArea PaintingArea { get; private set; }
    public Vector2 SpawnPosition { get; private set; }
    public Transform Transform => transform;

    public NpcIdleState IdleState { get; } = new();
    public NpcRoamState RoamState { get; } = new();
    public NpcDeadState DeadState { get; } = new();

    private static readonly int DeathAnimHash = Animator.StringToHash("Death");

    private void Awake() {
        Health = GetComponent<NpcHealthComponent>();
        Movement = GetComponent<NpcMovementComponent>();
    }

    /// <summary>
    /// Initializes the NPC with its painting context and starts its state machine.
    /// Must be called after instantiation before any Update runs.
    /// </summary>
    /// <param name="paintingArea">The painting area this NPC lives in.</param>
    public void Initialize(PlayablePaintingArea paintingArea) {
        PaintingArea = paintingArea;
        SpawnPosition = transform.position;

        Health.Initialize(stateData.maxHealth);
        Movement.Initialize(stateData.moveSpeed, paintingArea);

        Health.OnDeath += OnDeath;

        TransitionTo(IdleState);
    }

    private void Update() {
        _currentState?.Update(this);
    }

    /// <summary>
    /// Transitions the NPC to a new state, calling Exit on the old and Enter on the new.
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    public void TransitionTo(INpcState newState) {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    /// <summary>
    /// Triggers the death animation and transitions to the dead state.
    /// </summary>
    public void PlayDeathAnimation() {
        if (animator)
            animator.SetTrigger(DeathAnimHash);
    }

    private void OnDeath() {
        TransitionTo(DeadState);
    }

    private void OnDestroy() {
        if (Health != null)
            Health.OnDeath -= OnDeath;
    }

    #region IColorAffectable

    public void TakeDamage(int amount) => Health.TakeDamage(amount);
    public void InstantKill() => Health.InstantKill();
    public void ApplySlow(float factor) => Movement.ApplySlow(factor);
    public void RemoveSlow() => Movement.RemoveSlow();
    public void ApplyKnockback(Vector2 sourcePosition, float force) =>
        Movement.ApplyKnockback(sourcePosition, force);

    #endregion
}