using System.Collections.Generic;
using Graphs.StateMachine.ScriptableObjects;
namespace Graphs.StateMachine
{
    public class Transition : IStateComponent
    {
        private readonly StateSO m_targetState;
        private readonly List<Condition> m_conditions;

        public Transition(StateMachine stateMachine, TransitionStruct transition)
        {
            m_targetState = transition.targetState;
            m_conditions = new List<Condition>();
            foreach (ConditionStruct condition in transition.conditions)
            {
                m_conditions.Add(new Condition(stateMachine, condition));
            }
        }

        public bool TryGetTransition(out StateSO stateSO)
        {
            bool isMet = true;
            for (int i = 0; i < m_conditions.Count; i++)
            {
                isMet = isMet && m_conditions[i].IsMet();
            }
            stateSO = isMet ? m_targetState : null;
            return isMet;
        }

        public void OnStateEnter()
        {
            for (int i = 0; i < m_conditions.Count; i++)
            {
                m_conditions[i].m_decision.OnStateEnter();
            }
        }

        public void OnStateExit()
        {
            for (int i = 0; i < m_conditions.Count; i++)
            {
                m_conditions[i].m_decision.OnStateExit();
            }
        }
    }
}