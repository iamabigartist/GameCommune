using UnityEngine;
namespace Graphs.StateMachine.ScriptableObjects
{
    public abstract class ActionSO : ScriptableObject
    {
        internal Action GetAction(StateMachine stateMachine)
        {
            Action action = CreateAction();
            action.m_actionSO = this;
            action.Initialize( stateMachine );
            return action;
        }

        protected abstract Action CreateAction();
    }

    public abstract class ActionSO<T> : ActionSO where T : Action, new()
    {
        protected override Action CreateAction()
        {
            return new T();
        }
    }
}
