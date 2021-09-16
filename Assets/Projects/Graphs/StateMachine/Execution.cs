using Graphs.StateMachine.ScriptableObjects;
namespace Graphs.StateMachine
{
    public class Execution : IStateComponent
    {
        private readonly Action m_action;
        private readonly ExecutePoint m_executePoint;

        public Execution(StateMachine stateMachine, ExecutionStruct execution)
        {
            m_action = execution.action.GetAction(stateMachine);
            m_executePoint = execution.executePoint;
        }

        public void OnStateEnter()
        {
            if (m_executePoint == ExecutePoint.OnStateEnter)
            {
                m_action.OnAction();
            }
        }

        public void OnUpdate()
        {
            if (m_executePoint == ExecutePoint.OnUpdate)
            {
                m_action.OnAction();
            }
        }

        public void OnStateExit()
        {
            if (m_executePoint == ExecutePoint.OnStateExit)
            {
                m_action.OnAction();
            }
        }
    }
}
