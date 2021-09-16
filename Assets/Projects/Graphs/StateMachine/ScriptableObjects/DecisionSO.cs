using UnityEngine;
namespace Graphs.StateMachine.ScriptableObjects
{
    public abstract class DecisionSO : ScriptableObject
    {
        internal Decision GetDecision(StateMachine stateMachine)
        {
            Decision decision = CreateDecision();
            decision.m_decisionSO = this;
            decision.Initialize( stateMachine );
            return decision;
        }

        protected abstract Decision CreateDecision();
    }

    public abstract class DecisionSO<T> : DecisionSO where T : Decision, new()
    {
        protected override Decision CreateDecision()
        {
            return new T();
        }
    }
}
