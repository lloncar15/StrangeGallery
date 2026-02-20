using UnityEngine;

/// <summary>
/// Controls the player's 2D sprite representation inside paintings.
/// Subscribes to painting enter/exit events and manages its own visibility and positioning.
/// </summary>
public class PlayerSprite : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform footTransform;

    private float _footOffset;

    public float FootOffset => _footOffset;

    private void Awake() {
        _footOffset = footTransform.localPosition.y;
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
    /// <param name="position">World position to set</param>
    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    /// <summary>
    /// Sets the transform position so that the foot transform aligns with the given position.
    /// </summary>
    /// <param name="position">The position the foot transform should be at</param>
    private void SetPositionAtFoot(Vector3 position) {
        SetPosition(new Vector3(position.x, position.y - _footOffset, position.z));
    }

    /// <summary>
    /// Positions the sprite at the painting's spawn point and makes it visible.
    /// </summary>
    /// <param name="playablePaintingArea">The painting area being entered</param>
    private void OnEnteredPainting(PlayablePaintingArea playablePaintingArea) {
        SetPositionAtFoot(playablePaintingArea.SpawnPosition);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the sprite when exiting a painting.
    /// </summary>
    private void OnExitedPainting() {
        gameObject.SetActive(false);
    }
}