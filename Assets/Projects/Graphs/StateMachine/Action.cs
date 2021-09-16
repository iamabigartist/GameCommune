using Graphs.StateMachine.ScriptableObjects;
namespace Graphs.StateMachine
{
    public abstract class Action
    {
        internal ActionSO m_actionSO;

        public virtual void Initialize(StateMachine stateMachine) { }

        public virtual void OnAction() { }

        protected ActionSO actionSO => m_actionSO;
    }
}