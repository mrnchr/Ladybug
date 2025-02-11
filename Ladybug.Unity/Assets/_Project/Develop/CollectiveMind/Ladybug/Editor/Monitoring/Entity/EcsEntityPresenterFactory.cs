using CollectiveMind.Ladybug.Editor.Monitoring.World;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Zenject;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Entity
{
  public class EcsEntityPresenterFactory : IEcsEntityPresenterFactory
  {
    private readonly IInstantiator _instantiator;

    public EcsEntityPresenterFactory(IInstantiator instantiator)
    {
      _instantiator = instantiator;
    }

    public IEcsEntityPresenter Create(int entity, IEcsWorldPresenter parent, IEcsWorldWrapper wrapper)
    {
      return _instantiator.Instantiate<EcsEntityPresenter>(new object[] { entity, parent, wrapper });
    }
  }
}