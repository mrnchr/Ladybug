using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using LudensClub.GeoChaos.Editor.Monitoring.Universe;
using Zenject;

namespace LudensClub.GeoChaos.Editor.Monitoring.World
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