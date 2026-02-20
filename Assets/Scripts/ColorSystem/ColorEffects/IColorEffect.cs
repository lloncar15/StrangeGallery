public interface IColorEffect {
    ColorType ColorType { get; }

    /// <summary>
    /// Called when an NPC enters the trigger of a sprite carrying this effect.
    /// </summary>
    /// <param name="npc">The NPC that entered.</param>
    void OnEnter(NpcController npc);

    /// <summary>
    /// Called when an NPC exits the trigger of a sprite carrying this effect.
    /// </summary>
    /// <param name="npc">The NPC that exited.</param>
    void OnExit(NpcController npc);
}