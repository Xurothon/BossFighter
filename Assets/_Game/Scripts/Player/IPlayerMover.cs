using UnityEngine;

public interface IPlayerMover
{
    Transform Transform { get; }
    void SetDirection(Vector2 input);
}