using System;
using Core;
using UnityEngine;

namespace Conversations {
    [Serializable]
    public class ExampleCondition : YarnDialogueConditionBase {
        [SerializeField] private GameState state;
        
        public override bool CanStartDialogue() {
            return GameStateManager.GetCurrentState() == state;
        }
    }
}