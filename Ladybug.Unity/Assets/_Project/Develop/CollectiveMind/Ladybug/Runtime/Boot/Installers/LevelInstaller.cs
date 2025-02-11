using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class LevelInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindLineDrawer();
      BindLadybugRotator();

      BindRuntimeInitializer();
      
      InstallEcs();
    }

    private void BindLadybugRotator()
    {
      Container
        .Bind<ILadybugRotator>()
        .To<LadybugRotator>()
        .AsSingle();
    }

    private void BindLineDrawer()
    {
      Container
        .BindInterfacesTo<LineDrawer>()
        .AsSingle();
    }

    private void BindRuntimeInitializer()
    {
      Container
        .Bind<IRuntimeInitializer>()
        .To<RuntimeInitializer>()
        .AsSingle();
    }

    private void InstallEcs()
    {
      EcsInstaller.Install(Container);
    }
  }
}