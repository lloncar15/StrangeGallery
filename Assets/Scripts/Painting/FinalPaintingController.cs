using System.Collections.Generic;
using UnityEngine;

public class FinalPaintingController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private PlayablePaintingArea paintingArea;
    [SerializeField] private ColorAssignmentController colorAssignmentController;
    [SerializeField] private ColorPaletteUI colorPaletteUI;
    [SerializeField] private CollectedColors collectedColors;

    [Header("Paintable Sprites")]
    [SerializeField] private List<PaintableSprite> paintableSprites;

    [Header("NPC Spawning")]
    [SerializeField] private List<NpcSpawnData> npcSpawnData;

    private List<NpcController> _spawnedNpcs = new();
    private FinalPaintingPhase _currentPhase = FinalPaintingPhase.Paint;

    private void Start() {
        EnterPaintPhase();
    }

    private void OnEnable() {
        ColorAssignmentController.OnLockIn += OnLockIn;
    }

    private void OnDisable() {
        ColorAssignmentController.OnLockIn -= OnLockIn;
    }

    /// <summary>
    /// Activates the paint phase, enabling color assignment UI and disabling NPC activity.
    /// </summary>
    private void EnterPaintPhase() {
        _currentPhase = FinalPaintingPhase.Paint;
        colorPaletteUI.Initialize(collectedColors.GetCollectedColors());
        colorAssignmentController.Initialize(paintableSprites);
        colorAssignmentController.Enable();
        colorPaletteUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// Activates the chaos phase — locks sprites, starts moving sprites, and spawns NPCs.
    /// </summary>
    private void EnterChaosPhase() {
        _currentPhase = FinalPaintingPhase.Chaos;

        colorAssignmentController.Disable();
        colorPaletteUI.gameObject.SetActive(false);

        LockAllSprites();
        StartMovingSprites();
        SpawnNpcs();
    }

    /// <summary>
    /// Locks all paintable sprites so colors can no longer be modified.
    /// </summary>
    private void LockAllSprites() {
        foreach (PaintableSprite sprite in paintableSprites)
            sprite.Lock();
    }

    /// <summary>
    /// Activates movement on any paintable sprites that have a movement component.
    /// </summary>
    private void StartMovingSprites() {
        foreach (PaintableSprite sprite in paintableSprites) {
            if (sprite.TryGetComponent(out PaintableSpriteMovement movement))
                movement.StartMoving();
        }
    }

    /// <summary>
    /// Spawns all configured NPCs at their defined spawn positions and initializes them.
    /// </summary>
    private void SpawnNpcs() {
        foreach (NpcSpawnData spawnData in npcSpawnData) {
            if (!spawnData.prefab || !spawnData.spawnPoint) continue;

            NpcController npc = Instantiate(
                spawnData.prefab,
                spawnData.spawnPoint.position,
                Quaternion.identity
            );

            npc.Initialize(paintingArea);
            _spawnedNpcs.Add(npc);
        }
    }

    private void OnLockIn() {
        if (_currentPhase != FinalPaintingPhase.Paint) return;
        EnterChaosPhase();
    }
}

public enum FinalPaintingPhase {
    Paint,
    Chaos
}