using System;

namespace Conversations {
    [Serializable]
    public abstract class YarnDialogueConditionBase : IYarnDialogueCondition {
        public abstract bool CanStartDialogue();
    }
}