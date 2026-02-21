using System;
using UnityEngine;

/// <summary>
/// Controls the player's 2D sprite representation inside paintings.
/// Subscribes to painting enter/exit events and manages its own visibility and positioning.
/// Optionally implements IColorAffectable so color effects can affect the player.
/// </summary>
public class PlayerSprite : MonoBehaviour, IColorAffectable {
    [Header("References")]
    [SerializeField] private Transform footTransform;

    [Header("Color Effect Settings")]
    [SerializeField] private bool affectedByColorEffects;
    [SerializeField] private int maxHealth = 3;

    private float _footOffset;
    private int _currentHealth;
    private int _slowCount;
    private float _speedMultiplier = 1f;
    private PlayablePaintingArea _currentPaintingArea;

    public float FootOffset => _footOffset;
    public float SpeedMultiplier => _speedMultiplier;
    public Transform Transform => transform;

    public static event Action OnPlayerDeath;

    private void Awake() {
        _footOffset = footTransform.localPosition.y;
        _currentHealth = maxHealth;
    }

    private void OnEnable() {
        GameStateManager.OnEnteredPainting += OnEnteredPainting;
        GameStateManager.OnExitedPainting += OnExitedPainting;
    }

    private void OnDisable() {
        GameStateManager.OnEnteredPainting -= OnEnteredPainting;
        GameStateManager.OnExitedPainting -= OnExitedPainting;
    }

    /// <summary>
    /// Sets the transform position directly.
    /// </summary>
    /// <param name="position">World position to set.</param>
    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    /// <summary>
    /// Sets the transform position so that the foot transform aligns with the given position.
    /// </summary>
    /// <param name="position">The position the foot transform should be at.</param>
    private void SetPositionAtFoot(Vector3 position) {
        SetPosition(new Vector3(position.x, position.y - _footOffset, position.z));
    }

    /// <summary>
    /// Positions the sprite at the painting's spawn point, resets health, and makes it visible.
    /// </summary>
    /// <param name="playablePaintingArea">The painting area being entered.</param>
    private void OnEnteredPainting(PlayablePaintingArea playablePaintingArea) {
        _currentPaintingArea = playablePaintingArea;
        _currentHealth = maxHealth;
        _slowCount = 0;
        _speedMultiplier = 1f;
        SetPositionAtFoot(playablePaintingArea.SpawnPosition);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the sprite when exiting a painting.
    /// </summary>
    private void OnExitedPainting() {
        _currentPaintingArea = null;
        gameObject.SetActive(false);
    }

    #region IColorAffectable

    public void TakeDamage(int amount) {
        if (!affectedByColorEffects) return;
        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        if (_currentHealth <= 0)
            OnPlayerDeath?.Invoke();
    }

    public void InstantKill() {
        if (!affectedByColorEffects) return;
        _currentHealth = 0;
        OnPlayerDeath?.Invoke();
    }

    public void ApplySlow(float factor) {
        if (!affectedByColorEffects) return;
        _slowCount++;
        _speedMultiplier = factor;
    }

    public void RemoveSlow() {
        if (!affectedByColorEffects) return;
        _slowCount = Mathf.Max(0, _slowCount - 1);
        if (_slowCount == 0)
            _speedMultiplier = 1f;
    }

    public void ApplyKnockback(Vector2 sourcePosition, float force) {
        if (!affectedByColorEffects) return;

        Vector2 currentPos = transform.position;
        Vector2 direction = (currentPos - sourcePosition).normalized;

        if (direction == Vector2.zero)
            direction = Vector2.up;

        Vector3 newPosition = (Vector3)(currentPos + direction * force);

        if (_currentPaintingArea != null)
            newPosition = _currentPaintingArea.ClampToBounds(newPosition, _footOffset);

        transform.position = newPosition;
    }

    #endregion
}