using DG.Tweening;
using UnityEngine;

public class BossRotator : IRotator
{
    private readonly float _speedDegPerSec;
    private Tween _currentTween;
    
    public BossRotator(float speedDegPerSec)
    {
        _speedDegPerSec = speedDegPerSec;
    }
    
    public void RotateTowards(Transform target, Vector3 direction)
    {
        direction.y = 0;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
        
        _currentTween?.Kill();
        
        float angle = Quaternion.Angle(target.rotation, targetRot);
        if (angle < 1.5f) return;

        float duration = angle / _speedDegPerSec;
        _currentTween = target.DORotateQuaternion(targetRot, duration)
            .SetUpdate(true);
    }

    public void Stop() => _currentTween?.Kill();
}