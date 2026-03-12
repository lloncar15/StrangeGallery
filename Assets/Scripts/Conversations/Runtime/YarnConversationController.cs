using System;
using UnityEngine;
using Utils;
using Yarn.Unity;

namespace Conversations {
    /// <summary>
    /// Main controller for the conversation system using Yarn Spinner
    /// </summary>
    public class YarnConversationController : GenericSingleton<YarnConversationController> {
        [SerializeField] private YarnProject yarnProject;
        [SerializeField] private DialogueRunner dialogueRunner;

        private YarnDialogueProvider _activeProvider;

        public static event Action<YarnDialogueProvider> DialogueStarted;
        public static event Action<YarnDialogueProvider> DialogueEnded;

        protected override void Awake() {
            base.Awake();
            dialogueRunner?.onDialogueComplete?.AddListener(OnDialogueEnded);
        }

        public bool StartDialogue(YarnDialogueProvider provider, string nodeOverride = null) {
            if (IsDialogueRunning())
                return false;

            if (!provider.CanStartDialogue())
                return false;
            
            _activeProvider = provider;
            _activeProvider.InitializeStrategies(dialogueRunner.VariableStorage);

            string nodeName = nodeOverride ?? provider.DialogueReference.nodeName;

            if (nodeName == null)
                return false;
            
            dialogueRunner.StartDialogue(nodeName);
            
            DialogueStarted?.Invoke(provider);
            provider.OnThisDialogueStarted();
            return true;
        }

        public void StopDialogue() {
            if (!IsDialogueRunning())
                return;

            dialogueRunner.Stop();
        }

        private bool IsDialogueRunning() {
            return dialogueRunner && dialogueRunner.IsDialogueRunning;
        }

        private void OnDialogueEnded() {
            if (_activeProvider == null)
                return;
            
            DialogueEnded?.Invoke(_activeProvider);
            _activeProvider.OnThisDialogueEnded();
            _activeProvider = null;
        }

        private async YarnTask ExecuteStrategyAsync(int strategyId) {
            IYarnDialogueStrategy strategy = _activeProvider?.GetStrategy(strategyId);

            if (strategy == null) {
                return;
            }

            await strategy.Execute();
        }

        [YarnCommand("execute_strategy")]
        public static async YarnTask ExecuteStrategy(int strategyId) {
            await Instance.ExecuteStrategyAsync(strategyId);
        }
    }
}