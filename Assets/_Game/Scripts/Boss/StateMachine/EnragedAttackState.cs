using System;
using Cysharp.Threading.Tasks;

public class EnragedAttackState : BossStateBase
{
    private readonly BossContext _bossContext;
    private EnragedAttack _enragedAttack;
    private readonly float _attackDelay;
    private readonly IRotator _rotator;
    private bool _isAttacking;

    public EnragedAttackState(BossContext bossContext, float attackDelay = 1.5f)
    {
        _bossContext = bossContext;
        _attackDelay = attackDelay;
        _rotator = _bossContext.BossRotator;
        _enragedAttack = _bossContext.BossController.EnragedAttack;
    }

    protected override void OnEnter()
    {
        RunEnragedLoop().Forget();
    }

    protected override void OnExit()
    {
        _enragedAttack.Disable().Forget();
        _bossContext.BossWeakPointActivator.DeactivateAll();
    }

    public override void Tick(float deltaTime)
    {
        if(_isAttacking) return;
        
        _rotator.RotateTowards(_bossContext.BossTransform, 
            (_bossContext.PlayerTransform.position - _bossContext.BossTransform.position).normalized);
    }

    private async UniTaskVoid RunEnragedLoop()
    {
        try
        {
            _bossContext.BossWeakPointActivator.Activate(BodyPartName.Body);
            _bossContext.BossWeakPointActivator.Activate(BodyPartName.ArmLeft);
            _bossContext.BossWeakPointActivator.Activate(BodyPartName.LegRight);
            await WaitAsync(_attackDelay);
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
        float duration = _bossContext.BossAnimator.PlayAndGetDuration(BossAnimator.MagicAttackClipHash);
        await WaitAsync(duration * 0.25f);
        _enragedAttack.Activate();
        await WaitAsync(duration * 0.5f);
        await _enragedAttack.Disable();
        await WaitAsync(duration * 0.1f);
        _isAttacking = false;
    }
}