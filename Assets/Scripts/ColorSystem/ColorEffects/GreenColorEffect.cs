/// <summary>
/// Represents the green obstacle effect. Blocking is handled by PaintableSprite's
/// non-trigger collider, not by enter/exit events.
/// </summary>
public class GreenColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Green;

    /// <inheritdoc/>
    public void OnEnter(IColorAffectable target) { }

    /// <inheritdoc/>
    public void OnExit(IColorAffectable target) { }
}