using System;
using UnityEngine;
using Zenject;

public class BossHealth : MonoBehaviour, IHealth
{
    [SerializeField] private BodyPart[] _bodyParts;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _healthForSecondState;
    [Inject] private IBossStateMachine _stateMachine;
    private IHealth healthImplementation;
    public bool IsAlive => CurrentHP > 0;
    public float CurrentHP { get; private set; }
    public float MaxHP { get; private set; }
    public event Action<float> HPChanged;
    public event Action Died;

    private void Start()
    {
        MaxHP = _maxHealth;
        CurrentHP = MaxHP;
    }

    private void OnEnable()
    {
        foreach (BodyPart bodyPart in _bodyParts)
        {
            bodyPart.Hitted += OnHitted;
        } 
    }

    private void OnDisable()
    {
        foreach (BodyPart bodyPart in _bodyParts)
        {
            bodyPart.Hitted -= OnHitted;
        } 
    }

    private void OnHitted(float damage)
    {
        if (!IsAlive) return;

        CurrentHP = Mathf.Max(0, CurrentHP - damage);
        HPChanged?.Invoke(CurrentHP / _maxHealth);

        if (CurrentHP <= _healthForSecondState && _stateMachine.CurrentState is not EnragedAttackState)
        {
            _stateMachine.ChangeState<EnragedAttackState>();
            return;
        }

        if (CurrentHP <= 0)
        {
            _stateMachine.ChangeState<DeathState>();
            Died?.Invoke();
        }
    }
}