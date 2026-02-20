using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class UniverseEventBus
  {
    private readonly Dictionary<EcsWorld, WorldEventBus> _buses = new Dictionary<EcsWorld, WorldEventBus>();
    
    public IDisposable Subscribe<T>(EcsEntityWrapper entity, Action action) where T : struct
    {
      if (!_buses.TryGetValue(entity.World, out WorldEventBus bus))
      {
        bus = _buses[entity.World] = new WorldEventBus();
      }
      
      return bus.Subscribe<T>(entity.PackedEntity, action);
    }
    
    public void Publish<T>(EcsEntityWrapper entity) where T : struct
    {
      if (_buses.TryGetValue(entity.World, out WorldEventBus bus))
      {
        bus.Publish<T>(entity.PackedEntity);
      }
    }
  }
}