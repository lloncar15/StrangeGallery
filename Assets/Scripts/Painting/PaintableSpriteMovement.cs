using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PaintableSpriteMovement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private Vector2 initialDirection = Vector2.right;
    [SerializeField] private float speed = 3f;

    private Rigidbody2D _rb;
    private Vector2 _currentDirection;
    private bool _isActive;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
        _currentDirection = initialDirection.normalized;
    }

    private void FixedUpdate() {
        if (!_isActive) 
            return;
        
        _rb.linearVelocity = _currentDirection * speed;
    }

    /// <summary>
    /// Starts the sprite moving in its configured direction.
    /// </summary>
    public void StartMoving() => _isActive = true;

    /// <summary>
    /// Stops the sprite from moving.
    /// </summary>
    public void StopMoving() {
        _isActive = false;
        _rb.linearVelocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!_isActive) 
            return;
        
        Vector2 normal = collision.GetContact(0).normal;
        _currentDirection = Vector2.Reflect(_currentDirection, normal).normalized;
    }
}