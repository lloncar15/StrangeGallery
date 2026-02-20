using UnityEngine;

/// <summary>
/// Handles playing footstep sounds at regular intervals while the player is moving in FPS mode.
/// </summary>
public class FootstepsController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AudioSource footstepsSource;
    [SerializeField] private FootstepsConfig config;

    private float _stepTimer;
    private int _currentFootstepIndex;
    private int _footstepCount;

    private const float MOVEMENT_THRESHOLD = 0.01f;

    private void Start() {
        _footstepCount = config.footstepsSounds.Length;
    }

    private void Update() {
        if (GameStateManager.GetCurrentState() != GameState.FPS)
            return;

        HandleFootsteps();
    }

    /// <summary>
    /// Checks if the player is moving and advances the step timer.
    /// Plays a footstep sound when the timer exceeds the configured interval.
    /// </summary>
    private void HandleFootsteps() {
        Vector2 moveInput = InputController.Instance.MoveInput;

        bool isMoving = moveInput.sqrMagnitude > MOVEMENT_THRESHOLD;
        if (!isMoving) {
            _stepTimer = 0f;
            return;
        }

        _stepTimer += Time.deltaTime;
        if (_stepTimer >= config.stepInterval) {
            _stepTimer = 0f;
            PlayFootstep();
        }
    }

    /// <summary>
    /// Plays the next footstep sound in the cycle with a randomized pitch.
    /// </summary>
    private void PlayFootstep() {
        footstepsSource.pitch = Random.Range(config.pitchRange.x, config.pitchRange.y);
        SoundController.Instance.PlayOneShotSfx(footstepsSource, config.footstepsSounds[_currentFootstepIndex]);
        _currentFootstepIndex = (_currentFootstepIndex + 1) % _footstepCount;
    }
}