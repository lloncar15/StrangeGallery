using UnityEngine;

public class InteractablePainting : MonoBehaviour, IInteractable {
    [SerializeField] private PaintingObject paintingObject;
    
    private bool _isInRange;
    private PlayerInteractionController _controller;
    
    private void OnEnable() {
        GameStateManager.OnExitedPainting += OnExitedPainting;
    }

    private void OnDisable() {
        GameStateManager.OnExitedPainting -= OnExitedPainting;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        _isInRange = true;

        if (!CanBeInteracted()) {
            return;
        }

        _controller = other.GetComponent<PlayerInteractionController>();
        _controller.OnInteractionZoneEnter(this);
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        _isInRange = false;
        _controller.OnInteractionZoneExit();
        _controller = null;
    }
    
    /// <summary>
    /// Restores interactability when the player exits the painting.
    /// Handles the case where the player is force-moved back in front of the painting
    /// and may still be within the trigger collider.
    /// </summary>
    private void OnExitedPainting() {
        if (!_isInRange || _controller == null)
            return;
        
        _controller.OnInteractionZoneEnter(this);
    }

    public void Interact() {
        GameStateManager.Instance.EnterPainting(paintingObject);
    }
    
    /// <summary>
    /// Only interactable in FPS mode — prevents re-entry while already inside the painting.
    /// </summary>
    /// <returns>True if in FPS state and within trigger range</returns>
    public bool CanBeInteracted() {
        return _isInRange && GameStateManager.GetCurrentState() == GameState.FPS;
    }
}