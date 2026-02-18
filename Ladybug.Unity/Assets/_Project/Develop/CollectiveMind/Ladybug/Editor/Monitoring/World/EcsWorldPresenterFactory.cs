using CollectiveMind.Ladybug.Editor.Monitoring.Universe;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Zenject;

namespace CollectiveMind.Ladybug.Editor.Monitoring.World
{
  public class EcsWorldPresenterFactory : IEcsWorldPresenterFactory
  {
    private readonly IInstantiator _instantiator;

    public EcsWorldPresenterFactory(IInstantiator instantiator)
    {
      _instantiator = instantiator;
    }

    public IEcsWorldPresenter Create(IEcsWorldWrapper world, IEcsUniversePresenter parent)
    {
      return _instantiator.Instantiate<EcsWorldPresenter>(new object[] { world, parent });
    }
  }
}