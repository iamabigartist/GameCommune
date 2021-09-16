using Graphs.StateMachine.ScriptableObjects;
namespace Graphs.StateMachine
{
    public abstract class Decision : IStateComponent
    {
        internal DecisionSO m_decisionSO;

        public abstract bool Decide();

        public virtual void Initialize(StateMachine stateMachine) { }

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }

        public DecisionSO decisionSO => m_decisionSO;
    }
}
