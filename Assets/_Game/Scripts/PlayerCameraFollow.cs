using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _positionSmoothTime;
    
    [Inject] private IPlayerMover _target;

    private Vector3 _posVelocity = Vector3.zero;
    private float _rotAngleVelocity = 0f;

    private void LateUpdate()
    {
        if (_target?.Transform == null) return;
        
        Vector3 desiredPosition = _target.Transform.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _posVelocity, _positionSmoothTime);
    }
    
    private void OnEnable()
    {
        if (_target != null)
        {
            transform.position = _target.Transform.position + _offset;
            _posVelocity = Vector3.zero;
        }
    }
}