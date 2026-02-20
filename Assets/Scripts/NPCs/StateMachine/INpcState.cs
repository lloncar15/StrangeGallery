public interface INpcState {
    /// <summary>
    /// Called when this state is entered.
    /// </summary>
    /// <param name="npc">The NPC entering this state.</param>
    void Enter(NpcController npc);

    /// <summary>
    /// Called every frame while this state is active.
    /// </summary>
    /// <param name="npc">The NPC running this state.</param>
    void Update(NpcController npc);

    /// <summary>
    /// Called when this state is exited.
    /// </summary>
    /// <param name="npc">The NPC exiting this state.</param>
    void Exit(NpcController npc);
}