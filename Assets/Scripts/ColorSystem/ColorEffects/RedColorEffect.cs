/// <summary>
/// Deals damage to targets on enter.
/// </summary>
public class RedColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Red;

    private readonly int _damageAmount;

    public RedColorEffect(ColorEffectData data) {
        _damageAmount = data.damageAmount;
    }

    /// <inheritdoc/>
    public void OnEnter(IColorAffectable target) {
        target.TakeDamage(_damageAmount);
    }

    /// <inheritdoc/>
    public void OnExit(IColorAffectable target) { }
}