using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public class EcsInstaller : Installer<EcsInstaller>
  {
    public override void InstallBindings()
    {
      BindGameWorldWrapper();
      BindMessageWorldWrapper();

      BindEcsSystemsFactory();
      BindEcsSystemFactory();

      BindEcsDisposer();
      BindEcsEngine();

      BindEcsUniverse();
    }

    private void BindGameWorldWrapper()
    {
      Container
        .BindInterfacesAndSelfTo<GameWorldWrapper>()
        .AsSingle();
    }

    private void BindMessageWorldWrapper()
    {
      Container
        .BindInterfacesAndSelfTo<MessageWorldWrapper>()
        .AsSingle();
    }

    private void BindEcsSystemsFactory()
    {
      Container
        .Bind<IEcsSystemsFactory>()
        .To<EcsSystemsFactory>()
        .AsSingle();
    }

    private void BindEcsSystemFactory()
    {
      Container
        .Bind<IEcsSystemFactory>()
        .To<EcsSystemFactory>()
        .AsSingle();
    }

    private void BindEcsDisposer()
    {
      Container
        .BindInterfacesTo<EcsDisposer>()
        .AsSingle();
    }

    private void BindEcsEngine()
    {
      Container
        .BindInterfacesTo<EcsEngine>()
        .AsSingle();
    }

    private void BindEcsUniverse()
    {
      Container
        .Bind<IEcsUniverse>()
        .To<EcsUniverse>()
        .AsSingle();
    }
  }
}