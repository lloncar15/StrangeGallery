/// <summary>
/// Defines a color effect that activates on trigger enter/exit.
/// </summary>
public interface IColorEffect {
    ColorType ColorType { get; }

    /// <summary>
    /// Called when an affectable entity enters the trigger of a sprite carrying this effect.
    /// </summary>
    /// <param name="target">The entity that entered.</param>
    void OnEnter(IColorAffectable target);

    /// <summary>
    /// Called when an affectable entity exits the trigger of a sprite carrying this effect.
    /// </summary>
    /// <param name="target">The entity that exited.</param>
    void OnExit(IColorAffectable target);
}