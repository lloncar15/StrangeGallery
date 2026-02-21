/// <summary>
/// Slows targets while they remain inside the trigger.
/// </summary>
public class BlueColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Blue;

    private readonly float _slowFactor;

    public BlueColorEffect(ColorEffectData data) {
        _slowFactor = data.slowFactor;
    }

    /// <inheritdoc/>
    public void OnEnter(IColorAffectable target) {
        target.ApplySlow(_slowFactor);
    }

    /// <inheritdoc/>
    public void OnExit(IColorAffectable target) {
        target.RemoveSlow();
    }
}