using UnityEngine;

public class NpcDeadState : INpcState {
    private const float DESTROY_DELAY = 2f;

    // ReSharper disable Unity.PerformanceAnalysis
    /// <inheritdoc/>
    public void Enter(NpcController npc) {
        npc.Movement.Stop();

        Collider2D col = npc.GetComponent<Collider2D>();
        if (col) col.enabled = false;

        npc.PlayDeathAnimation();
        Object.Destroy(npc.gameObject, DESTROY_DELAY);
    }

    /// <inheritdoc/>
    public void Update(NpcController npc) { }

    /// <inheritdoc/>
    public void Exit(NpcController npc) { }
}