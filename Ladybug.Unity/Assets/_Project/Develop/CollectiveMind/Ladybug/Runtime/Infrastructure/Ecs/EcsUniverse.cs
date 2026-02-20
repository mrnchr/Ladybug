using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsUniverse : IEcsUniverse
  {
    private readonly List<IEcsWorldWrapper> _worldWrappers;
    private readonly UniverseEventBus _universeEventBus;
    
    public IEcsWorldWrapper GameWrapper { get; }
    public IEcsWorldWrapper MessageWrapper { get; }
    
    public EcsWorld Game => GameWrapper.World;
    public EcsWorld Message => MessageWrapper.World;

    public EcsUniverse(List<IEcsWorldWrapper> worldWrappers)
    {
      _worldWrappers = worldWrappers;
      _universeEventBus = new UniverseEventBus();

      GameWrapper = _worldWrappers.Find(x => x.Name == EcsConstants.Worlds.GAME);
      MessageWrapper = _worldWrappers.Find(x => x.Name == EcsConstants.Worlds.MESSAGE);
    }

    public EcsWorld.Mask FilterGame<TComponent>() where TComponent : struct
    {
      return Game.Filter<TComponent>();
    }

    public EcsWorld.Mask FilterMessage<TComponent>() where TComponent : struct
    {
      return Message.Filter<TComponent>();
    }

    public IDisposable Subscribe<T>(EcsEntityWrapper entity, Action action) where T : struct
    {
      return _universeEventBus.Subscribe<T>(entity, action);
    }

    public void Publish<T>(EcsEntityWrapper entity) where T : struct
    {
      _universeEventBus.Publish<T>(entity);
    }
  }
}