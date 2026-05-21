using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Input")]
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _shoot;
    [SerializeField] private InputActionReference _mousePosition;
    
    [Header("Pooling")]
    [SerializeField] private Bullet _projectilePrefab;
    [SerializeField] private int _poolInitialSize;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private Transform _poolRoot;
    
    [Header("Player")]
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerControllerFacade _playerControllerFacade;
    [SerializeField] private float _rotationSpeed;
    
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        
        Container.Bind<IInputService>().To<InputService>().AsSingle()
            .WithArguments(_move, _shoot, _mousePosition);
        
        Container.Bind<IObjectPool<Bullet>>()
            .To<ObjectPool<Bullet>>()
            .AsSingle()
            .WithArguments(_projectilePrefab, _poolRoot, _poolMaxSize, Container)
            .OnInstantiated<ObjectPool<Bullet>>((ctx, pool) =>
            {
                pool.WarmupAsync(_poolInitialSize).Forget();
            });
        
        Container.Bind<IProjectileFactory>().To<ProjectileFactory>().AsSingle().WhenInjectedInto<PlayerShooter>();
        
        Container.Bind<IPlayerMover>().To<PlayerMover>().FromInstance(_playerMover).AsSingle();
        Container.Bind<IPlayerAnimator>().To<PlayerAnimator>().FromInstance(_playerAnimator).AsSingle();
        Container.Bind<IPlayerShooter>().To<PlayerShooter>().AsSingle();
        Container.Bind<IHealth>().To<PlayerHealth>().FromInstance(_playerHealth).AsSingle().
            WhenInjectedInto(typeof(UIPlayerHealthBar), typeof(PlayerControllerFacade));
        
        Container.Bind<IRotator>().To<PlayerRotator>()
            .AsSingle().WithArguments(_rotationSpeed).WhenInjectedInto<PlayerControllerFacade>();
        
        Container.Bind<PlayerControllerFacade>().FromInstance(_playerControllerFacade);
    }
}