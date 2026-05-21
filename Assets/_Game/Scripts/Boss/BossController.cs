using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BossController : MonoBehaviour
{
    [SerializeField] private Sword _sword;
    [SerializeField] private EnragedAttack _enragedAttack;
    public IBossStateMachine StateMachine { get; private set; }
    public Sword Sword => _sword;
    public EnragedAttack EnragedAttack => _enragedAttack;

    public event System.Action OnPlayerTriggerEntered;
    public event System.Action OnDefeated;

    [Inject]
    public void Construct(IBossStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    private void Start()
    {
        StateMachine.ChangeState<WaitForPlayerState>();
    }

    private void Update()
    {
        StateMachine.Tick(Time.deltaTime);
    }

    public void OnPlayerEnteredTrigger()
    {
        OnPlayerTriggerEntered?.Invoke();
    }
    
    public async void OnBossDefeated()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        OnDefeated?.Invoke();
    }
}