using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public abstract class BossStateBase : IBossState
{
    protected CancellationTokenSource Cts { get; private set; }

    public virtual void Enter()
    {
        Cts = new CancellationTokenSource();
        OnEnter();
    }

    public virtual void Exit()
    {
        OnExit();
        Cts?.Cancel();
        Cts?.Dispose();
        Cts = null;
    }

    public abstract void Tick(float deltaTime);
    protected abstract void OnEnter();
    protected abstract void OnExit();

    protected async UniTask WaitAsync(float seconds)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: Cts.Token);
    }
}