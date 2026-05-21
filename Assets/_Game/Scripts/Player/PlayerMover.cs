using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour, IPlayerMover
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody _rb;
    private Vector2 _currentInput;
    
    public Transform Transform => transform;

    public void SetDirection(Vector2 input) => 
        _currentInput = input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_rb == null) return;
        Vector3 moveDir = new Vector3(_currentInput.x, 0, _currentInput.y);
        _rb.linearVelocity = moveDir.normalized * _speed;
    }
}