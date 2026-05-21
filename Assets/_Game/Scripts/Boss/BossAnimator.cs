using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossAnimator : MonoBehaviour, IBossAnimator
{
    public static readonly int AxeAttackClipHash = Animator.StringToHash("axe_attack");
    public static readonly int AxeAttackComboClipHash = Animator.StringToHash("axe_attack_combo");
    public static readonly int MagicAttackClipHash = Animator.StringToHash("magic_attack");
    public static readonly int MagicAttackFromGroundClipHash = Animator.StringToHash("magic_attack_from_ground");
    public static readonly int VictoryClipHash = Animator.StringToHash("victory");
    public static readonly int DeathClipHash = Animator.StringToHash("death");
    public static readonly int StayClipHash = Animator.StringToHash("stay");
    
    [SerializeField] private AnimationClip _axeAttackClip; 
    [SerializeField] private AnimationClip _axeAttackComboClip; 
    [SerializeField] private AnimationClip _magicAttackClip; 
    [SerializeField] private AnimationClip _magicAttackFromGroundClip; 
    [SerializeField] private AnimationClip _victoryClip; 
    [SerializeField] private AnimationClip _deathClip; 
    [SerializeField] private AnimationClip _stayClip; 
    
    private Animator _animator;
    private Dictionary<int, AnimationClip> _lookup;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _lookup = new Dictionary<int, AnimationClip>()
        {
            { AxeAttackClipHash, _axeAttackClip },
            { AxeAttackComboClipHash, _axeAttackComboClip },
            { MagicAttackClipHash, _magicAttackClip },
            { MagicAttackFromGroundClipHash, _magicAttackFromGroundClip },
            { VictoryClipHash, _victoryClip },
            { DeathClipHash, _deathClip },
            { StayClipHash, _stayClip },
        };
    }
    
    public float PlayAndGetDuration(int hash)
    {
        if (!_lookup.TryGetValue(hash, out var clip))
        {
            Debug.LogWarning($"[AnimationPlayer] Animation with hash {hash} not found");
            return 0f;
        }
        
        _animator.Play(hash);
        
        return clip.length;
    }
}
