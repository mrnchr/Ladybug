using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class DeleteSystem<TComponent, TWrapper> : IEcsRunSystem
    where TComponent : struct, IEcsComponent
    where TWrapper : IEcsWorldWrapper
  {
    private readonly EcsWorld _world;
    private readonly EcsEntities _deleteEntities;

    protected DeleteSystem(TWrapper worldWrapper, Action<EcsWorld.Mask> clarifier = null)
    {
      _world = worldWrapper.World;

      EcsWorld.Mask deleteMask = _world.Filter<TComponent>();
      clarifier?.Invoke(deleteMask);
      _deleteEntities = deleteMask.Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper entity in _deleteEntities)
        entity.Del<TComponent>();
    }
  }

  public class DeleteSystem<TComponent> : DeleteSystem<TComponent, GameWorldWrapper>
    where TComponent : struct, IEcsComponent
  {
    protected DeleteSystem(GameWorldWrapper gameWorldWrapper, Action<EcsWorld.Mask> clarifier = null)
      : base(gameWorldWrapper, clarifier)
    {
    }
  }
}