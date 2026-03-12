using System;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Conversations {
    /// <summary>
    /// Abstract base class that implements IYarnDialogueStrategy used for creating concrete strategies.
    /// </summary>
    [Serializable]
    public abstract class YarnDialogueStrategyBase : IYarnDialogueStrategy {
        [SerializeField] private int strategyId;
        
        public int StrategyId => strategyId;

        protected IVariableStorage variableStorage;

        public void SetVariableStorage(IVariableStorage storage) {
            variableStorage = storage;
        }
        
        public abstract YarnTask Execute();
    }
}