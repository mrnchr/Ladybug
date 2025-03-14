using CollectiveMind.Ladybug.Runtime.Advertisement;
using CollectiveMind.Ladybug.Runtime.Boot.Initializers;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement.Boot;
using CollectiveMind.Ladybug.Runtime.UI.Defeat;
using CollectiveMind.Ladybug.Runtime.UI.HUD;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class LevelInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindAdService();
      
      BindPauseSwitcher();
      
      InstallWindow();
      
      BindRuntimeInitializer();

      BindReviver();
      
      InstallCollisions();
      
      BindLineDrawer();
      BindLadybugRotator();
      InstallObstacle();

      InstallEcs();

      BindViewFactory();
      
      BindCanvasService();
      
      InstallHUD();

      BindLevelInitializer();

#if UNITY_EDITOR
      EditorBridge.InstallGameplay(Container);
#endif
    }

    private void BindAdService()
    {
      Container
        .Bind<IAdService>()
        .To<AdService>()
        .AsSingle();
    }

    private void BindPauseSwitcher()
    {
      Container
        .Bind<IPauseSwitcher>()
        .To<PauseSwitcher>()
        .AsSingle();
    }

    private void InstallWindow()
    {
      WindowInstaller.Install(Container);
    }

    private void BindRuntimeInitializer()
    {
      Container
        .Bind<IRuntimeInitializer>()
        .To<RuntimeInitializer>()
        .AsSingle();
    }

    private void BindReviver()
    {
      Container
        .Bind<Reviver>()
        .AsSingle();
    }

    private void InstallCollisions()
    {
      CollisionsInstaller.Install(Container);
    }

    private void BindLineDrawer()
    {
      Container
        .BindInterfacesTo<LineDrawer>()
        .AsSingle();
    }

    private void BindLadybugRotator()
    {
      Container
        .Bind<ILadybugRotator>()
        .To<LadybugRotator>()
        .AsSingle();
    }

    private void InstallObstacle()
    {
      ObstacleInstaller.Install(Container);
    }

    private void InstallEcs()
    {
      EcsInstaller.Install(Container);
    }

    private void BindViewFactory()
    {
      Container
        .Bind<IViewFactory>()
        .To<ViewFactory>()
        .AsSingle();
    }

    private void BindCanvasService()
    {
      Container
        .Bind<ICanvasService>()
        .To<CanvasService>()
        .AsSingle();
    }

    private void InstallHUD()
    {
      HUDInstaller.Install(Container);
    }

    private void BindLevelInitializer()
    {
      Container
        .BindInterfacesTo<LevelInitializer>()
        .AsSingle();
    }
  }
}