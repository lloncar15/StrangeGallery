public class RedColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Red;

    private readonly int _damageAmount;

    public RedColorEffect(ColorEffectData data) {
        _damageAmount = data.damageAmount;
    }

    /// <inheritdoc/>
    public void OnEnter(NpcController npc) {
        // npc.Health.TakeDamage(_damageAmount);
    }

    /// <inheritdoc/>
    public void OnExit(NpcController npc) { }
}