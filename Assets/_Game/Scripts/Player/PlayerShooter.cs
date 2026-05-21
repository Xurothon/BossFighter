using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerShooter : IPlayerShooter
{
    private readonly IProjectileFactory _factory;
    private readonly float _fireRate = 0.2f;
    private bool _canShoot = true;

    public PlayerShooter(IProjectileFactory factory) => 
        _factory = factory;

    public async UniTask ShootAsync(Vector3 spawnPos, Quaternion spawnRot, Vector3 direction, float bulletSpeed, CancellationToken ct = default)
    {
        if (!_canShoot) return;
        _canShoot = false;

        _factory.Create(spawnPos, spawnRot, direction, bulletSpeed);
        await UniTask.Delay(System.TimeSpan.FromSeconds(_fireRate), cancellationToken: ct);
        _canShoot = true;
    }
}