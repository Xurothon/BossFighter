using System;
using Cysharp.Threading.Tasks;
using Zenject;

public class AttackState : BossStateBase
{
    [Inject] private readonly BossContext _bossContext;
    [Inject] private readonly IUIBossHealthBar _healthBar;
    private readonly IProjectileFactory _factory;
    private readonly IRotator _rotator;
    private readonly float _attackDelay;
    private bool _isAttacking;

    public AttackState(BossContext bossContext, float attackDelay = 2f)
    {
        _bossContext = bossContext;
        _attackDelay = attackDelay;
        _rotator = _bossContext.BossRotator;
        _factory = _bossContext.Factory;
    }

    protected override void OnEnter()
    {
        RunAttackLoop().Forget();
    }

    protected override void OnExit()
    {
        _bossContext.Sword.DisableSword();
        _bossContext.BossWeakPointActivator.Deactivate(BodyPartName.Head);
        _bossContext.BossWeakPointActivator.Deactivate(BodyPartName.ArmRight);
    }

    public override void Tick(float deltaTime)
    {
        if(_isAttacking) return;
        
        _rotator.RotateTowards(_bossContext.BossTransform, 
            (_bossContext.PlayerTransform.position - _bossContext.BossTransform.position).normalized);
    }

    private async UniTaskVoid RunAttackLoop()
    {
        try
        {
            _bossContext.BossWeakPointActivator.Activate(BodyPartName.Head);
            _bossContext.BossWeakPointActivator.Activate(BodyPartName.ArmRight);
            _healthBar.Active();
            float duration = _bossContext.BossAnimator.PlayAndGetDuration(BossAnimator.StayClipHash);
            await WaitAsync(duration);
            while (!Cts.Token.IsCancellationRequested)
            {
                await PerformAttack();
                await WaitAsync(_attackDelay);
            }
        }
        catch (OperationCanceledException) { }
    }

    private async UniTask PerformAttack()
    {
        _isAttacking = true;
        await _bossContext.Sword.MakeBig();
        float duration = _bossContext.BossAnimator.PlayAndGetDuration(BossAnimator.AxeAttackClipHash);
        await WaitAsync(duration * 0.4f);
        _factory.Create(_bossContext.Sword.SwordTargetTransform.position, 
            _bossContext.Sword.SwordTargetTransform.rotation, 
            _bossContext.Sword.SwordTargetTransform.forward, 
            _bossContext.Sword.SwordAttackSpeed);
        await _bossContext.Sword.MakeSmall();
        await WaitAsync(duration * 0.6f);
        _isAttacking = false;
    }
}