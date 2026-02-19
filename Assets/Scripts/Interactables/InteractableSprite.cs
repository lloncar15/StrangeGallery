using UnityEngine;

public abstract class InteractableSprite : MonoBehaviour, IInteractable {
    private bool _isInRange;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player"))
            return;
        
        _isInRange = true;
        
        if (!CanBeInteracted())
            return;
        
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        controller.OnInteractionZoneEnter(this);
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Player"))
            return;

        _isInRange = false;
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        controller.OnInteractionZoneExit();
    }

    public abstract void Interact();
    
    /// <summary>
    /// Only interactable in Painting (2D) mode.
    /// </summary>
    /// <returns>True if in Painting state and within trigger range</returns>
    public bool CanBeInteracted() {
        return _isInRange && GameStateManager.GetCurrentState() == GameState.Painting;
    }

}