using System;
using Core;
using Painting.Runtime;
using UnityEngine;
using Yarn.Unity;

namespace Conversations {
    [Serializable]
    public class ExampleStrategy : YarnDialogueStrategyBase {
        [SerializeField] private Transform targetPosition;
        [SerializeField] private PlayerSprite player;

        public override async YarnTask Execute() {
            // await player.MoveTo(targetPosition.position);
            await YarnTask.Yield();
            GameStateManager.Instance.ExitPainting();
        }
    }
}