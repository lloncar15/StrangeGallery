using System;
using Core;
using Interactables;
using Painting;
using UnityEngine;
using Yarn.Unity;

namespace Conversations {
    [Serializable]
    public class ExitPaintingStrategy : YarnDialogueStrategyBase {
        [SerializeField] private PaintingFlowController paintingFlowController;
        [SerializeField] private InteractableConversationSprite conversationSprite;

        public override async YarnTask Execute() {
            conversationSprite.LockInteraction();
            await YarnTask.Yield();
            paintingFlowController.ExitPainting();
        }
    }
}