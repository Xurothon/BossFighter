using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BossStateMachine : IBossStateMachine, IInitializable
{
    private readonly Dictionary<Type, IBossState> _states = new();
    [Inject] private DiContainer _container;
    private IBossState _currentState;

    public IBossState CurrentState => _currentState;
    
    public void ChangeState<TState>() where TState : IBossState
    {
        var targetType = typeof(TState);
        if (!_states.TryGetValue(targetType, out var targetState))
        {
            Debug.LogError($"[BossStateMachine] State {targetType.Name} is not registered!");
            return;
        }
        
        if (_currentState is TState) return;

        _currentState?.Exit();
        _currentState = _states.TryGetValue(typeof(TState), out var state) ? state : null;
        _currentState?.Enter();
    }

    public void Tick(float deltaTime)
    {
        _currentState?.Tick(deltaTime);
    }

    public void Initialize()
    {
        _states[typeof(WaitForPlayerState)] = _container.Instantiate<WaitForPlayerState>();
        _states[typeof(AttackState)] = _container.Instantiate<AttackState>();
        _states[typeof(EnragedAttackState)] = _container.Instantiate<EnragedAttackState>();
        _states[typeof(DeathState)] = _container.Instantiate<DeathState>();
    }
}