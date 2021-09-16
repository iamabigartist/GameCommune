using Graphs.StateMachine.ScriptableObjects;
namespace Graphs.StateMachine
{
    public class State
    {
        internal Execution[] m_executions;
        internal Transition[] m_transitions;

        public State() { }

        public void OnStateEnter()
        {
            for (int i = 0; i < m_executions.Length; i++)
            {
                m_executions[i].OnStateEnter();
            }
            for (int i = 0; i < m_transitions.Length; i++)
            {
                m_transitions[i].OnStateEnter();
            }
        }

        public void OnUpdate()
        {
            for (int i = 0; i < m_executions.Length; i++)
            {
                m_executions[i].OnUpdate();
            }
        }

        public void OnStateExit()
        {
            for (int i = 0; i < m_executions.Length; i++)
            {
                m_executions[i].OnStateExit();
            }
            for (int i = 0; i < m_transitions.Length; i++)
            {
                m_transitions[i].OnStateExit();
            }
        }

        public bool TryGetTransition(out StateSO stateSO)
        {
            stateSO = null;

            for (int i = 0; i < m_transitions.Length; i++)
            {
                if (m_transitions[i].TryGetTransition(out stateSO))
                {
                    break;
                }
            }
            return stateSO != null;
        }
    }
}
