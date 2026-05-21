public class DeathState : BossStateBase
{
    private readonly BossContext _bossContext;

    public DeathState(BossContext bossContext)
    {
        _bossContext = bossContext;
    }

    protected override void OnEnter()
    {
        PlayDeathSequence();
    }

    protected override void OnExit() { }

    public override void Tick(float deltaTime) { }

    private async void PlayDeathSequence()
    {
        float duration = _bossContext.BossAnimator.PlayAndGetDuration(BossAnimator.DeathClipHash);
        await WaitAsync(duration);
        _bossContext.BossController.OnBossDefeated();
    }
}