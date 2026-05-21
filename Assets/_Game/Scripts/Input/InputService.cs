using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService, IDisposable
{
    private readonly InputActionReference _move;
    private readonly InputActionReference _shoot;
    private readonly InputActionReference _mousePosition;
    public event Action<Vector2> OnMove;
    public event Action<Vector2> OnMouseScreenPosition;
    public event Action OnFire;
    
    public InputService(InputActionReference move, InputActionReference shoot, InputActionReference mousePosition)
    {
        _move = move;
        _shoot = shoot;
        _mousePosition = mousePosition;
    }

    public void Enable()
    {
        _move.action.Enable();
        _shoot.action.Enable();
        _move.action.performed += OnMovePerformed;
        _move.action.canceled += OnMoveCanceled;
        _shoot.action.performed += OnFirePerformed;
        _mousePosition.action.performed += OnMouseScreenPositionPerformed;
    }

    public void Disable()
    {
        _move.action.Disable();
        _shoot.action.Disable();
        _move.action.performed -= OnMovePerformed;
        _move.action.canceled -= OnMoveCanceled;
        _shoot.action.performed -= OnFirePerformed;
        _mousePosition.action.performed -= OnMouseScreenPositionPerformed;
    }

    public void Dispose()
    {
        _move.action.Dispose();
        _shoot.action.Dispose();
        _mousePosition.action.Dispose();
    }
    
    private void OnMovePerformed(InputAction.CallbackContext obj) => 
        OnMove?.Invoke(obj.ReadValue<Vector2>());

    private void OnMoveCanceled(InputAction.CallbackContext obj) => 
        OnMove?.Invoke(Vector2.zero);

    private void OnFirePerformed(InputAction.CallbackContext obj) => 
        OnFire?.Invoke();

    private void OnMouseScreenPositionPerformed(InputAction.CallbackContext obj) => 
        OnMouseScreenPosition?.Invoke(obj.ReadValue<Vector2>());
}