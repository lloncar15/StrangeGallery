using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Conversations {
    /// <summary>
    /// Class that provides dialogue references to the controller and any dialogue strategies needed
    /// </summary>
    [Serializable]
    public class YarnDialogueProvider : MonoBehaviour {
        [SerializeField] private DialogueReference dialogueReference;
        [SerializeField] [SerializeReference] private List<YarnDialogueConditionBase> conditions = new();
        [SerializeField] [SerializeReference] private List<YarnDialogueStrategyBase> strategies = new();

        private YarnConversationController _controller;
        private Dictionary<int, YarnDialogueStrategyBase> _strategyLookup;
        
        public DialogueReference DialogueReference => dialogueReference;

        public event Action DialogueWithThisProviderStarted;
        public event Action DialogueWithThisProviderEnded;

        private void Awake() {
            ConstructStrategyLookup();
        }

        private void Start() {
            _controller = YarnConversationController.Instance;
        }

        /// <summary>
        /// Constructs the dictionary for strategy lookup and throws an exception if two strategy IDs are the same.
        /// </summary>
        /// <exception cref="Exception">Duplicate strategy IDs</exception>
        private void ConstructStrategyLookup() {
            _strategyLookup = new Dictionary<int, YarnDialogueStrategyBase>();
            foreach (YarnDialogueStrategyBase strategy in strategies) {
                if (!_strategyLookup.TryAdd(strategy.StrategyId, strategy)) {
                    throw new Exception($"Duplicate strategy id {strategy.StrategyId}");
                }
            }
        }

        public void TriggerDialogue(string nodeOverride = null) {
            _controller.StartDialogue(this, nodeOverride);
        }

        public void ExitDialogue() {
            _controller.StopDialogue();
        }

        public IYarnDialogueStrategy GetStrategy(int strategyId) {
            return _strategyLookup.GetValueOrDefault(strategyId, null);
        }

        public bool CheckConditions() {
            return conditions.All(condition => condition.CanStartDialogue());
        }

        public void InitializeStrategies(IVariableStorage storage) {
            foreach (YarnDialogueStrategyBase strategy in strategies) {
                strategy.SetVariableStorage(storage);
            }
        }
        
        public void OnThisDialogueStarted() {
            DialogueWithThisProviderStarted?.Invoke();
        }

        public void OnThisDialogueEnded() {
            DialogueWithThisProviderEnded?.Invoke();
        }
    }
}