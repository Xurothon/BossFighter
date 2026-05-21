public interface IBossStateMachine
{
    IBossState CurrentState { get; }
    void ChangeState<TState>() where TState : IBossState;
    void Tick(float deltaTime);
}