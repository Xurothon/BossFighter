using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int _maxHealth;
    public float CurrentHP { get; private set; }
    public float MaxHP { get; private set; }
    public bool IsAlive => CurrentHP > 0;
    public event Action<float> HPChanged;
    public event Action Died;
    
    private void Start()
    {
        MaxHP = _maxHealth;
        CurrentHP = MaxHP;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;
        
        CurrentHP -= damage;
        HPChanged?.Invoke(CurrentHP / _maxHealth);

        if (CurrentHP <= 0)
        {
            Died?.Invoke();
        }
    }
}