public class BlackColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Black;

    /// <inheritdoc/>
    public void OnEnter(NpcController npc) {
        // npc.Health.InstantKill();
    }

    /// <inheritdoc/>
    public void OnExit(NpcController npc) { }
}