using Yarn.Unity;

namespace Conversations {
    public interface IYarnDialogueStrategy {
        int StrategyId { get; }
        public YarnTask Execute();
    }
}