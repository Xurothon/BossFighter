using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerControllerFacade : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private LayerMask _aimLayerMask = -1;
    [SerializeField] private float _aimDistance;
    [SerializeField] float _bulletSpeed;  
    
    [Inject] private IInputService _input;
    [Inject] private IPlayerMover _mover;
    [Inject] private IPlayerShooter _shooter;
    [Inject] private IRotator _rotator;
    [Inject] private IPlayerAnimator _animator; 
    [Inject] private IHealth _health; 
    [Inject] private Camera _camera;
    
    private Vector2 _mouseScreenPos;
    private bool _isDied;
    private CancellationTokenSource _cts = new();
    
    public event Action OnDefeated;

    private void OnEnable()
    {
        _health.Died += OnDied;
        _input.OnMove += HandleMove;
        _input.OnMouseScreenPosition += MouseScreenPosition;
        _input.OnFire += HandleFire;
        _input.Enable();
    }

    private async void OnDied()
    {
        OnDefeated?.Invoke();
        _isDied = true;
        _health.Died -= OnDied;
        _input.OnMove -= HandleMove;
        _input.OnMouseScreenPosition -= MouseScreenPosition;
        _input.OnFire -= HandleFire;
        _input.Disable();
        _cts?.Cancel();
        _animator.Play(PlayerAnimator.DeathClipHash);
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
    }

    private void OnDisable()
    {
        OnDied();
    }

    private void HandleMove(Vector2 dir)
    {
        if(_isDied) return;
        _mover.SetDirection(dir);
        _animator.UpdateLocomotion(dir.magnitude);
    }
    
    private void MouseScreenPosition(Vector2 screenPos)
    {
        if(_isDied) return;
        _mouseScreenPos = screenPos;
        _rotator.RotateTowards(transform, CalculateAimDirection());
    }

    private async void HandleFire()
    {
        Vector3 direction = CalculateAimDirection();
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        
        await _shooter.ShootAsync(_muzzle.position, rotation, direction, _bulletSpeed, _cts.Token);
    }
    
    private Vector3 CalculateAimDirection()
    {
        Ray ray = _camera.ScreenPointToRay(_mouseScreenPos);

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, _aimDistance, _aimLayerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(_aimDistance);
        }
        
        Vector3 direction = (targetPoint - _muzzle.position).normalized;

        return direction;
    }
}