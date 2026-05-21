using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BossInstaller : MonoInstaller
{
    [SerializeField] private BossController _bossController;
    [SerializeField] private BossActivationTrigger _bossActivationTrigger;
    [SerializeField] private BossAnimator _bossAnimator;
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private UIBossHealthBar _UIBossHealthBar;
    [SerializeField] private BossWeakPointSystem _bossWeakPointSystem;
    [SerializeField] private float _rotationSpeed;
    
    [Header("Pooling")]
    [SerializeField] private SwordAttack _projectilePrefab;
    [SerializeField] private int _poolInitialSize;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private Transform _poolRoot;
    
    public override void InstallBindings()
    {
        Container.Bind<IObjectPool<SwordAttack>>()
            .To<ObjectPool<SwordAttack>>()
            .AsSingle()
            .WithArguments(_projectilePrefab, _poolRoot, _poolMaxSize, Container)
            .OnInstantiated<ObjectPool<SwordAttack>>((ctx, pool) =>
            {
                pool.WarmupAsync(_poolInitialSize).Forget();
            });
        
        Container.Bind<IProjectileFactory>().To<SwordProjectileFactory>().AsSingle().WhenInjectedInto<BossContext>();
        Container.Bind<IBossWeakPointActivator>().To<BossWeakPointSystem>().FromInstance(_bossWeakPointSystem).AsSingle();
        
        Container.BindInterfacesAndSelfTo<BossStateMachine>().AsSingle();
        
        Container.Bind<BossController>().FromInstance(_bossController);
        Container.Bind<IBossAnimator>().To<BossAnimator>().FromInstance(_bossAnimator).AsSingle();
        
        Container.Bind<IHealth>().To<BossHealth>().FromInstance(bossHealth).AsSingle().
            WhenInjectedInto(typeof(BossContext), typeof(UIBossHealthBar));
        Container.Bind<BossContext>().AsSingle();
        Container.Bind<IUIBossHealthBar>().To<UIBossHealthBar>().FromInstance(_UIBossHealthBar).AsSingle();
        Container.Bind<BossActivationTrigger>().FromInstance(_bossActivationTrigger).AsSingle();
        Container.Bind<IRotator>().To<BossRotator>()
            .AsSingle().WithArguments(_rotationSpeed).WhenInjectedInto<BossContext>();
    }
}