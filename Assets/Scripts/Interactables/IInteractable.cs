public interface IInteractable {
    /// <summary>
    /// Called when the player interacts with this object.
    /// </summary>
    void Interact();
    
    /// <summary>
    /// Returns whether this object can currently be interacted with.
    /// </summary>
    /// <returns>True if interaction is possible</returns>
    bool CanBeInteracted();
}