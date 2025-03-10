using CollectiveMind.Ladybug.Runtime.Boot.Initializers;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Gameplay.Obstacle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement.Boot;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class LevelInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindPauseSwitcher();
      
      InstallWindow();
      
      BindRuntimeInitializer();
      
      BindLineDrawer();
      BindLadybugRotator();
      InstallObstacle();

      InstallEcs();

      BindViewFactory();

      BindLevelInitializer();

#if UNITY_EDITOR
      EditorBridge.InstallGameplay(Container);
#endif
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

    private void BindLevelInitializer()
    {
      Container
        .BindInterfacesTo<LevelInitializer>()
        .AsSingle();
    }
  }
}