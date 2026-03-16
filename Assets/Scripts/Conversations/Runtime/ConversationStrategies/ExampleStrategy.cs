using System;
using Core;
using Painting;
using UnityEngine;
using Yarn.Unity;

namespace Conversations {
    [Serializable]
    public class ExampleStrategy : YarnDialogueStrategyBase {
        [SerializeField] private GameObject objectToAppear;

        public override async YarnTask Execute() {
            // await player.MoveTo(targetPosition.position);
            await YarnTask.Yield();
            // GameStateManager.Instance.ExitPainting();
        }
    }
}