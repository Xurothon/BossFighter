using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour, IProjectile
{
    [SerializeField] private float _lifetime;
    [SerializeField] private ParticleSystem _particleSystem;
    [Inject] private IObjectPool<Bullet> _pool;
    private CancellationTokenSource _lifetimeCts;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 direction, float speed)
    {
        _lifetimeCts?.Cancel();
        _lifetimeCts = new CancellationTokenSource();
        
        _rb.linearVelocity = direction * speed;
        _rb.angularVelocity = Vector3.zero;
        _particleSystem.Play();
        
        LifetimeAsync(_lifetimeCts.Token).Forget();
    }

    private async UniTask LifetimeAsync(CancellationToken ct)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_lifetime), cancellationToken: ct);
            ReturnToPool();
        }
        catch (OperationCanceledException) { }
    }

    public void ReturnToPool()
    {
        _lifetimeCts?.Cancel();
        _pool?.Return(this);
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void OnDisable() => _lifetimeCts?.Cancel();
}