using UnityEngine;

public class NpcMovementComponent : MonoBehaviour {
    private float _baseSpeed;
    private float _currentSpeed;
    private float _slowFactor;
    private int _slowCount;

    private PlayablePaintingArea _paintingArea;

    /// <summary>
    /// Initializes movement with a base speed and painting area reference for bounds clamping.
    /// </summary>
    /// <param name="baseSpeed">The NPC's unmodified movement speed.</param>
    /// <param name="paintingArea">The painting area for bounds clamping. Can be null.</param>
    public void Initialize(float baseSpeed, PlayablePaintingArea paintingArea = null) {
        _baseSpeed = baseSpeed;
        _currentSpeed = baseSpeed;
        _paintingArea = paintingArea;
    }

    public float CurrentSpeed => _currentSpeed;

    /// <summary>
    /// Applies a slow multiplier stack. Multiple slows use the latest factor.
    /// </summary>
    /// <param name="factor">Multiplier between 0 and 1.</param>
    public void ApplySlow(float factor) {
        _slowCount++;
        _slowFactor = factor;
        _currentSpeed = _baseSpeed * _slowFactor;
    }

    /// <summary>
    /// Removes one slow stack. Restores base speed when all stacks are removed.
    /// </summary>
    public void RemoveSlow() {
        _slowCount = Mathf.Max(0, _slowCount - 1);
        if (_slowCount == 0)
            _currentSpeed = _baseSpeed;
    }

    /// <summary>
    /// Stops all movement by setting speed to zero.
    /// </summary>
    public void Stop() {
        _currentSpeed = 0f;
    }

    /// <summary>
    /// Applies a knockback impulse, displacing the NPC away from the source position.
    /// Result is clamped to painting bounds if available.
    /// </summary>
    /// <param name="sourcePosition">World position of the knockback origin.</param>
    /// <param name="force">Distance of the knockback displacement.</param>
    public void ApplyKnockback(Vector2 sourcePosition, float force) {
        Vector2 currentPos = transform.position;
        Vector2 direction = (currentPos - sourcePosition).normalized;

        if (direction == Vector2.zero)
            direction = Vector2.up;

        Vector3 newPosition = (Vector3)(currentPos + direction * force);

        if (_paintingArea != null)
            newPosition = _paintingArea.ClampToBounds(newPosition, 0f);

        transform.position = newPosition;
    }
}