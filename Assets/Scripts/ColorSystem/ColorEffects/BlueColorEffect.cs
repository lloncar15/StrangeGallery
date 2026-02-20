public class BlueColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Blue;

    private readonly float _slowFactor;

    public BlueColorEffect(ColorEffectData data) {
        _slowFactor = data.slowFactor;
    }

    /// <inheritdoc/>
    public void OnEnter(NpcController npc) {
        // npc.Movement.ApplySlow(_slowFactor);
    }

    /// <inheritdoc/>
    public void OnExit(NpcController npc) {
        // npc.Movement.RemoveSlow();
    }
}