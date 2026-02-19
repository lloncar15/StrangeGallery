using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour {
    public static event Action OnInteractionZoneEntered;
    public static event Action OnInteractionZoneExited;
    
    private IInteractable _interactableInRange;

    private void OnEnable() {
        InputController.OnInteractPressed += Interact;
    }

    private void OnDisable() {
        InputController.OnInteractPressed -= Interact;
    }

    /// <summary>
    /// Called when the player enters an interaction zone (both in 3D or 2D world space).
    /// Stores the interactable reference and fires the zone entered event for UI.
    /// </summary>
    /// <param name="interactable">The interactable object in range</param>
    public void OnInteractionZoneEnter(IInteractable interactable) {
        _interactableInRange = interactable;
        OnInteractionZoneEntered?.Invoke();
    }

    /// <summary>
    /// Called when the player exits an interaction zone (both in 3D or 2D world space).
    /// Clears the interactable reference and fires the zone exited event for UI.
    /// </summary>
    public void OnInteractionZoneExit() {
        _interactableInRange = null;
        OnInteractionZoneExited?.Invoke();
    }

    /// <summary>
    /// Attempts to interact with the current interactable in range.
    /// Delegates all state and availability checks to the interactable itself via CanBeInteracted().
    /// </summary>
    private void Interact() {
        if (_interactableInRange == null)
            return;

        if (!_interactableInRange.CanBeInteracted())
            return;
        
        _interactableInRange.Interact();
    }
}