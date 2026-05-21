using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IPlayerShooter
{
    UniTask ShootAsync(Vector3 spawnPos, Quaternion spawnRot, Vector3 direction, float bulletSpeed, CancellationToken ct = default);
}