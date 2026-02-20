public class GreenColorEffect : IColorEffect {
    public ColorType ColorType => ColorType.Green;

    /// <inheritdoc/>
    public void OnEnter(NpcController npc) { }

    /// <inheritdoc/>
    public void OnExit(NpcController npc) { }
}