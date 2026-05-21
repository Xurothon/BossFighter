using Zenject;

public class WaitForPlayerState : BossStateBase
{
    [Inject] private BossController _bossController;
    protected override void OnEnter()
    {
        _bossController.OnPlayerTriggerEntered += HandlePlayerEntered;
    }

    protected override void OnExit()
    {
        _bossController.OnPlayerTriggerEntered -= HandlePlayerEntered;
    }

    public override void Tick(float deltaTime) { }

    private void HandlePlayerEntered()
    {
        _bossController.StateMachine.ChangeState<AttackState>();
    }
}