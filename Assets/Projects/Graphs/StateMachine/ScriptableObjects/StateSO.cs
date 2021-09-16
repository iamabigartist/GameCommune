using System;
using UnityEngine;
namespace Graphs.StateMachine.ScriptableObjects
{
    public class StateSO : ScriptableObject
    {
        [Tooltip( "Actions to do in this state every frame. Actions with smaller indices would be done earlier." )]
        public ExecutionStruct[] executions;

        [Tooltip( "Transitions to check in this state every frame. Transitions with smaller indices would have higher priority." )]
        public TransitionStruct[] transitions;

        internal void InitState(StateMachine stateMachine)
        {
            State state = new State();
            state.m_executions = GetExecutions( stateMachine );
            state.m_transitions = GetTransitions( stateMachine );
            stateMachine.m_createdStates[this] = state;
        }

        Execution[] GetExecutions(StateMachine stateMachine)
        {
            Execution[] newExecutions = new Execution[executions.Length];
            for (int i = 0; i < newExecutions.Length; i++)
            {
                newExecutions[i] = new Execution( stateMachine, executions[i] );
            }
            return newExecutions;
        }

        Transition[] GetTransitions(StateMachine stateMachine)
        {
            Transition[] newTransitions = new Transition[transitions.Length];
            for (int i = 0; i < newTransitions.Length; i++)
            {
                newTransitions[i] = new Transition( stateMachine, transitions[i] );
            }
            return newTransitions;
        }
    }

    public enum ExecutePoint
    {
        OnStateEnter,
        OnUpdate,
        OnStateExit
    }

    [Serializable]
    public struct ExecutionStruct
    {
        public ExecutePoint executePoint;
        public ActionSO action;
    }

    [Serializable]
    public struct TransitionStruct
    {
        [Tooltip( "The conditions you want to met for the transition. (The transition happens if and only if all the conditions are met.)" )]
        public ConditionStruct[] conditions;

        [Tooltip( "The state you want to transition to if the conditions are satisfied." )]
        public StateSO targetState;
    }

    [Serializable]
    public struct ConditionStruct
    {
        [Tooltip( "The decision to be done." )]
        public DecisionSO decision;

        [Tooltip( "The result expected for the decision." )]
        public Result expectedResult;
    }

    public enum Result
    {
        True,
        False
    }
}
