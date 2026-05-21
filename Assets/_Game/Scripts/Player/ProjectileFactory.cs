using UnityEngine;

public class ProjectileFactory : IProjectileFactory
{
    private readonly IObjectPool<Bullet> _pool;
    public ProjectileFactory(IObjectPool<Bullet> pool) => _pool = pool;

    public IProjectile Create(Vector3 position, Quaternion rotation, Vector3 direction, float speed)
    {
        var proj = _pool.Get();
        proj.transform.SetPositionAndRotation(position, rotation);
        proj.Fire(direction, speed);
        return proj;
    }
}