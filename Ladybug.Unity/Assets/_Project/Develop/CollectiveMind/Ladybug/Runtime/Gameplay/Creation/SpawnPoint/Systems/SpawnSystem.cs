using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Components;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Systems
{
  public class SpawnSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly IViewFactory _factory;
    private readonly EcsEntities _spawns;

    public SpawnSystem(IEcsUniverse universe, IViewFactory factory)
    {
      _universe = universe;
      _factory = factory;

      _spawns = _universe
        .FilterGame<SpawnPointTag>()
        .Inc<Spawnable>()
        .Collect();
    }
      
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper spawn in _spawns)
      {
        GameObjectConverter instance = _factory.Create<GameObjectConverter>(spawn.Get<SpawnedEntityId>().Id);
        instance.SetEntity(_universe.Game, _universe.Game.NewEntity());

        instance.EntityWrapper
          .Add((ref Spawned spawned) => spawned.Spawn = spawn.PackedEntity);

        spawn.Del<Spawnable>();
      }
    }
  }
}