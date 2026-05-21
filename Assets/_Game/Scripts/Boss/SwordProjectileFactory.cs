using UnityEngine;

public class SwordProjectileFactory : IProjectileFactory
{
    private readonly IObjectPool<SwordAttack> _pool;
    public SwordProjectileFactory(IObjectPool<SwordAttack> pool) => _pool = pool;

    public IProjectile Create(Vector3 position, Quaternion rotation, Vector3 direction, float speed)
    {
        var proj = _pool.Get();
        proj.transform.SetPositionAndRotation(position, rotation);
        proj.Fire(direction, speed);
        return proj;
    }
}