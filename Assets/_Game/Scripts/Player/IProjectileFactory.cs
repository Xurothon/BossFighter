using UnityEngine;

public interface IProjectileFactory
{
    IProjectile Create(Vector3 position, Quaternion rotation, Vector3 direction, float speed);
}