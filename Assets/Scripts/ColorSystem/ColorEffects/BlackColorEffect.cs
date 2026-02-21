/// <summary>
/// Instantly kills any target on enter.
/// </summary>
public class BlackColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Black;

    /// <inheritdoc/>
    public void OnEnter(IColorAffectable target) {
        target.InstantKill();
    }

    /// <inheritdoc/>
    public void OnExit(IColorAffectable target) { }
}