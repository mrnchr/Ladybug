using Leopotam.EcsLite;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsSystemFactory : IEcsSystemFactory
  {
    private readonly IInstantiator _instantiator;

    public EcsSystemFactory(IInstantiator instantiator)
    {
      _instantiator = instantiator;
    }

    public IEcsSystem Create<TSystem>() where TSystem : IEcsSystem
    {
      return _instantiator.Instantiate<TSystem>();
    }
  }
}