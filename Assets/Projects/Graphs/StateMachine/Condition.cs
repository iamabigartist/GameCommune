using Graphs.StateMachine.ScriptableObjects;
namespace Graphs.StateMachine
{
    public class Condition
    {
        internal Decision m_decision;
        internal bool m_expectedResult;

        public Condition(StateMachine stateMachine, ConditionStruct condition)
        {
            m_decision = condition.decision.GetDecision(stateMachine);
            m_expectedResult = condition.expectedResult == Result.True;
        }

        public bool IsMet()
        {
            return m_decision.Decide() == m_expectedResult;
        }
    }
}
