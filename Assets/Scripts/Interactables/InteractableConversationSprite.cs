using System;
using Conversations;
using UnityEngine;

namespace Interactables {
    public class InteractableConversationSprite : InteractableSprite {
        [SerializeField] private YarnDialogueProvider dialogueProvider;

        private bool _isInteractionInProgress;

        private void OnEnable() {
            dialogueProvider.DialogueWithThisProviderEnded += OnDialogueWithProviderEnded;
        }

        private void OnDisable() {
            dialogueProvider.DialogueWithThisProviderEnded -= OnDialogueWithProviderEnded;
        }

        public override void Interact() {
            if (dialogueProvider.StartDialogue())
                _isInteractionInProgress = true;
        }

        public override bool CanBeInteracted() {
            return base.CanBeInteracted() && dialogueProvider.CanStartDialogue() && !_isInteractionInProgress;
        }

        private void OnDialogueWithProviderEnded() {
            _isInteractionInProgress = false;
        }
    }
}