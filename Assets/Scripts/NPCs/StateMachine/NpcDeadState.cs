public class NpcDeadState : INpcState {
    /// <inheritdoc/>
    public void Enter(NpcController npc) {
        npc.PlayDeathAnimation();
    }

    /// <inheritdoc/>
    public void Update(NpcController npc) { }

    /// <inheritdoc/>
    public void Exit(NpcController npc) { }
}