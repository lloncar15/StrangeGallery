using UnityEngine;

public class NpcIdleState : INpcState {
    private float _waitDuration;
    private float _waitTimer;

    /// <inheritdoc/>
    public void Enter(NpcController npc) {
        _waitDuration = Random.Range(npc.StateData.minWaitTime, npc.StateData.maxWaitTime);
        _waitTimer = 0f;
    }

    /// <inheritdoc/>
    public void Update(NpcController npc) {
        _waitTimer += Time.deltaTime;
        if (_waitTimer >= _waitDuration)
            npc.TransitionTo(npc.RoamState);
    }

    /// <inheritdoc/>
    public void Exit(NpcController npc) { }
}