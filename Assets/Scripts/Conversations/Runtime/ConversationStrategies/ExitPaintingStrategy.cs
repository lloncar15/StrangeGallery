using System;
using Core;
using Painting;
using UnityEngine;
using Yarn.Unity;

namespace Conversations {
    [Serializable]
    public class ExitPaintingStrategy : YarnDialogueStrategyBase {
        [SerializeField] private PaintingFlowController paintingFlowController;

        public override async YarnTask Execute() {
            await YarnTask.Yield();
            paintingFlowController.ExitPainting();
        }
    }
}