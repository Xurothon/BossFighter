using System;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private float _startDamageMultiplier;
    [SerializeField] private Transform _animationTransform;
    public Transform AnimationTransform => _animationTransform;
    public Action<float> Hitted;
    private float _damage;

    private void Start()
    {
        _damage = _startDamageMultiplier;
    }

    public void ActiveWeakPoint(float damageMultiplier)
    {
        _damage = damageMultiplier;
    }

    public void DeactiveWeakPoint()
    {
        _damage = _startDamageMultiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            Hitted?.Invoke(_damage);
            bullet.ReturnToPool();
        }
    }
}