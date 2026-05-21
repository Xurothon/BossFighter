using System;
using UnityEngine;

public interface IInputService
{
    event Action<Vector2> OnMove;
    event Action OnFire;
    event Action<Vector2> OnMouseScreenPosition;
    void Enable();
    void Disable();
    void Dispose();
}