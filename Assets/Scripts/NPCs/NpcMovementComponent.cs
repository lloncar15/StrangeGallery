using UnityEngine;

public class NpcMovementComponent : MonoBehaviour {
    private float _baseSpeed;
    private float _currentSpeed;
    private bool _isSlowed;

    /// <summary>
    /// Initializes movement with a base speed defined by the NPC's config.
    /// </summary>
    /// <param name="baseSpeed">The NPC's unmodified movement speed.</param>
    public void Initialize(float baseSpeed) {
        _baseSpeed = baseSpeed;
        _currentSpeed = baseSpeed;
    }

    public float CurrentSpeed => _currentSpeed;

    /// <summary>
    /// Applies a slow multiplier to the NPC's speed. Only one slow can be active at a time.
    /// </summary>
    /// <param name="factor">Multiplier between 0 and 1 (e.g. 0.5 = half speed).</param>
    public void ApplySlow(float factor) {
        if (_isSlowed) return;
        _isSlowed = true;
        _currentSpeed = _baseSpeed * factor;
    }

    /// <summary>
    /// Removes the active slow and restores base speed.
    /// </summary>
    public void RemoveSlow() {
        _isSlowed = false;
        _currentSpeed = _baseSpeed;
    }
}