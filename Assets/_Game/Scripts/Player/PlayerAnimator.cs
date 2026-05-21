using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
{
    public static readonly int WinClipHash = Animator.StringToHash("win");
    public static readonly int DeathClipHash = Animator.StringToHash("death");
    
    private static readonly int _speedHash = Animator.StringToHash("speed");
    private static readonly int _dieHash = Animator.StringToHash("die");
    
    private float _lastMagnitude;
    private bool _isMoving;
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Play(int hash)
    {
        if (hash == DeathClipHash)
        {
            _animator.SetTrigger(_dieHash);
        }
        _animator.Play(hash);
    }
    
    public void UpdateLocomotion(float magnitude)
    {
        if (magnitude > 0.1f && !_isMoving)
        {
            _isMoving = true;
            _animator.SetFloat(_speedHash, 1f);
        }
        else if (magnitude < 0.05f && _isMoving)
        {
            _isMoving = false;
            _animator.SetFloat(_speedHash, 0f);
        }
    }
}