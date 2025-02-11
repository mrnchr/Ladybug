using System.Collections.Generic;
using System.Linq;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsSystemsFactory : IEcsSystemsFactory
  {
    private readonly List<IEcsWorldWrapper> _wrappers;
    private readonly IEcsDisposer _disposer;

    public EcsSystemsFactory(List<IEcsWorldWrapper> wrappers, IEcsDisposer disposer)
    {
      _wrappers = wrappers;
      _disposer = disposer;
    }

    public EcsSystems Create(string defaultName)
    {
      IEcsWorldWrapper defaultWrapper = _wrappers.Find(x => x.Name == defaultName);
      var otherWrappers = _wrappers.Where(x => x != defaultWrapper).ToList();
      var instance = new EcsSystems(defaultWrapper.World);
      foreach (IEcsWorldWrapper wrapper in otherWrappers)
        instance.AddWorld(wrapper.World, wrapper.Name);

      _disposer.Systems.Add(instance);
      return instance;
    }
  }
}