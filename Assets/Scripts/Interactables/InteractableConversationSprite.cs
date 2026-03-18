using System;
using Conversations;
using Painting;
using UnityEngine;

namespace Interactables {
    public class InteractableConversationSprite : InteractableSprite {
        [SerializeField] private YarnDialogueProvider dialogueProvider;

        private bool _isInteractionInProgress;
        private bool _isInteractionLocked;

        private void OnEnable() {
            dialogueProvider.DialogueWithThisProviderEnded += OnDialogueWithProviderEnded;
            PaintingFlowController.ExitedPainting += UnlockInteraction;
        }

        private void OnDisable() {
            dialogueProvider.DialogueWithThisProviderEnded -= OnDialogueWithProviderEnded;
            PaintingFlowController.ExitedPainting -= UnlockInteraction;
        }

        public override void Interact() {
            if (dialogueProvider.StartDialogue())
                _isInteractionInProgress = true;
        }

        public override bool CanBeInteracted() {
            return base.CanBeInteracted()
                   && dialogueProvider.CanStartDialogue()
                   && !_isInteractionInProgress
                   && !_isInteractionLocked;
        }

        private void OnDialogueWithProviderEnded() {
            _isInteractionInProgress = false;
        }

        public void LockInteraction() {
            _isInteractionLocked = true;
        }

        public void UnlockInteraction() {
            _isInteractionLocked = false;
        }
    }
}