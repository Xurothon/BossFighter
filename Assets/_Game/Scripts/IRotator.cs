using UnityEngine;

public interface IRotator
{
    void RotateTowards(Transform target, Vector3 direction);
    void Stop();
}