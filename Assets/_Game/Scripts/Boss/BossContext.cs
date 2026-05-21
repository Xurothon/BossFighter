using UnityEngine;
using Zenject;

public class BossContext
{
    public IHealth Health { get; }
    public IBossAnimator BossAnimator { get; }
    public IBossStateMachine BossStateMachine { get; }
    public IRotator BossRotator { get; }
    public IProjectileFactory Factory { get; }
    public BossController BossController { get; }
    public IBossWeakPointActivator BossWeakPointActivator { get; }
    public Sword Sword { get; }
    public Transform PlayerTransform { get; set; }
    public Transform BossTransform { get; set; }
    
    [Inject]
    public BossContext(IHealth health, IBossAnimator animator, BossController bossController, IRotator bossRotator, 
        IPlayerMover playerMover, IProjectileFactory factory, IBossWeakPointActivator bossWeakPointActivator)
    {
        Health = health;
        BossAnimator = animator;
        BossController = bossController;
        BossStateMachine = bossController.StateMachine;
        Sword = bossController.Sword;
        BossRotator = bossRotator;
        PlayerTransform = playerMover.Transform;
        BossTransform = bossController.transform;
        Factory = factory;
        BossWeakPointActivator = bossWeakPointActivator;
    }
}
