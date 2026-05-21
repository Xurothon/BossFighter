using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnragedAttack :MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float damageInterval;
    [SerializeField] private ParticleSystem _particleSystem;
    private CancellationTokenSource _cts;
    private bool _isProcessing;
    private PlayerHealth _playerHealth;

    public void Activate()
    {
        gameObject.SetActive(true);
        var main = _particleSystem.main;
        main.loop = true;
    }

    public async UniTask Disable()
    {
        var main = _particleSystem.main;
        main.loop = false;
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        gameObject.SetActive(false);
    }
    
    private void StartTakingDamage()
    {
        if (_isProcessing) return;

        _isProcessing = true;
        _cts = new CancellationTokenSource();
        
        DamageLoopAsync(_cts.Token).Forget();
    }

    private void StopTakingDamage()
    {
        if (_cts == null) return;
        _playerHealth = null;
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _isProcessing = false;
    }
    
    private async UniTaskVoid DamageLoopAsync(CancellationToken explicitCt)
    {
        try
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                explicitCt,
                gameObject.GetCancellationTokenOnDestroy()
            );

            while (true)
            {
                ApplyDamage();
                await UniTask.Delay(TimeSpan.FromSeconds(damageInterval), cancellationToken: explicitCt);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _cts?.Dispose();
            _cts = null;
            _isProcessing = false;
        }
    }
    
    private void ApplyDamage()
    {
        _playerHealth.TakeDamage(_damage);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            _playerHealth = playerHealth;
            StartTakingDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            StopTakingDamage();
        }
    }

    private void OnDisable()
    {
        StopTakingDamage();
    }
}