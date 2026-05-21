using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public static class TweenUniTaskExtensions
{
    public static UniTask ToUniTask(this Tween tween, CancellationToken ct = default)
    {
        if (ct.IsCancellationRequested)
        {
            tween.Kill();
            return UniTask.FromCanceled(ct);
        }

        var tcs = new UniTaskCompletionSource();
        var reg = ct.Register(() =>
        {
            tween.Kill();
            tcs.TrySetCanceled();
        });

        tween.OnComplete(() =>
        {
            reg.Dispose();
            tcs.TrySetResult();
        }).OnKill(() =>
        {
            reg.Dispose();
            tcs.TrySetCanceled();
        });

        return tcs.Task;
    }
}