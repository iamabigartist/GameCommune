namespace Graphs.StateMachine
{
    interface IStateComponent
    {
        void OnStateEnter();

        void OnStateExit();
    }
}
