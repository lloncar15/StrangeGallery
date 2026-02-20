using UnityEngine;

public class NpcRoamState : INpcState {
    private Vector2 _targetPosition;
    private bool _hasTarget;

    private const float ARRIVAL_THRESHOLD = 0.15f;

    /// <inheritdoc/>
    public void Enter(NpcController npc) {
        PickNewTarget(npc);
    }

    /// <inheritdoc/>
    public void Update(NpcController npc) {
        if (!_hasTarget) {
            npc.TransitionTo(npc.IdleState);
            return;
        }

        Vector2 currentPosition = npc.transform.position;
        Vector2 direction = (_targetPosition - currentPosition).normalized;
        float step = npc.Movement.CurrentSpeed * Time.deltaTime;

        npc.transform.position = Vector2.MoveTowards(currentPosition, _targetPosition, step);

        if (Vector2.Distance(currentPosition, _targetPosition) <= ARRIVAL_THRESHOLD) {
            _hasTarget = false;
            npc.TransitionTo(npc.IdleState);
        }
    }

    /// <inheritdoc/>
    public void Exit(NpcController npc) { }

    /// <summary>
    /// Picks a random target position within the roam radius, clamped to the painting bounds.
    /// </summary>
    /// <param name="npc">The NPC picking a new roam target.</param>
    private void PickNewTarget(NpcController npc) {
        Vector2 origin = npc.SpawnPosition;
        Vector2 randomOffset = Random.insideUnitCircle * npc.StateData.roamRadius;
        Vector3 candidate = new Vector3(origin.x + randomOffset.x, origin.y + randomOffset.y, npc.transform.position.z);

        _targetPosition = npc.PaintingArea.ClampToBounds(candidate, 0f);
        _hasTarget = true;
    }
}