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
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class CoreInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
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
        .Bind(typeof(IDisposable), typeof(LineDrawer))
        .To<LineDrawer>()
        .AsSingle();
      
      ObstacleInstaller.Install(Container);

      EcsInstaller.Install(Container);

      Container
        .Bind<IViewFactory>()
        .To<ViewFactory>()
        .AsSingle();
      
      Container
        .Bind<ICanvasService>()
        .To<CanvasService>()
        .AsSingle();

      Container
        .BindInterfacesAndSelfTo<CameraShakeController>()
        .AsSingle();

      Container
        .BindInterfacesTo<LadybugCameraObserver>()
        .AsSingle();
      
      Container
        .BindInterfacesAndSelfTo<SessionService>()
        .AsSingle();

      Container
        .BindInterfacesAndSelfTo<GameplayUpdater>()
        .AsSingle();
      
      Container
        .Bind<GameSwitcher>()
        .AsSingle();
      
      Container
        .BindInterfacesTo<CoreInitializer>()
        .AsSingle();

#if UNITY_EDITOR
      EditorBridge.InstallGameplay(Container);
#endif
    }
  }
}