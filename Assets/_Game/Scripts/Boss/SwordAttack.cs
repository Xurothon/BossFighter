using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class SwordAttack : MonoBehaviour, IProjectile
{
    [SerializeField] private float _lifetime;
    [SerializeField] private float _damage;
    [Inject] private IObjectPool<SwordAttack> _pool;
    private Rigidbody _rb;
    private CancellationTokenSource _lifetimeCts;
    
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
        
        LifetimeAsync(_lifetimeCts.Token).Forget();
    }
    
    private async UniTask LifetimeAsync(CancellationToken ct)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_lifetime), cancellationToken: ct);
            gameObject.SetActive(false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(_damage);
            ReturnToPool();
        }
    }

    private void OnDisable() => _lifetimeCts?.Cancel();
}