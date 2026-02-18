using System;
using CollectiveMind.Ladybug.Runtime.Advertisement;
using CollectiveMind.Ladybug.Runtime.Boot.Initializers;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class CoreInstaller : MonoInstaller
  {
    [SerializeField]
    private CoreInitializer _initializer;
    
    public override void InstallBindings()
    {
      Container
        .Bind<EntityFactory>()
        .AsSingle();
      
      Container
        .Bind<IAdService>()
        .To<AdService>()
        .AsSingle();
      
      WindowInstaller.Install(Container);
      
      Container
        .Bind<IRuntimeInitializer>()
        .To<RuntimeInitializer>()
        .AsSingle();

      Container
        .Bind<GameSessionData>()
        .AsSingle();

      CollisionsInstaller.Install(Container);

      Container
        .Bind<CameraService>()
        .AsSingle();
      
      Container
        .Bind(typeof(IDisposable), typeof(LineDrawer))
        .To<LineDrawer>()
        .AsSingle();
      
      ObstacleInstaller.Install(Container);

      EcsInstaller.Install(Container);

      Container
        .Bind<ICanvasService>()
        .To<CanvasService>()
        .AsSingle();

      Container
        .BindInterfacesAndSelfTo<CameraShakeController>()
        .AsSingle();

      Container
        .BindInterfacesAndSelfTo<OutOfViewObserver>()
        .AsSingle();

      Container
        .BindInterfacesTo<HealthObserver>()
        .AsSingle()
        .NonLazy();
      
      Container
        .BindInterfacesAndSelfTo<SessionService>()
        .AsSingle();

      Container
        .BindInterfacesAndSelfTo<GameplayUpdater>()
        .AsSingle();
      
      Container
        .Bind<GameSessionController>()
        .AsSingle();

      Container
        .BindInterfacesAndSelfTo<CoreCreationRecipeRegistrar>()
        .AsSingle();
      
      Container
        .BindInterfacesTo<CoreInitializer>()
        .FromInstance(_initializer)
        .AsSingle();

#if UNITY_EDITOR
      EditorBridge.InstallGameplay(Container);
#endif
    }
  }
}