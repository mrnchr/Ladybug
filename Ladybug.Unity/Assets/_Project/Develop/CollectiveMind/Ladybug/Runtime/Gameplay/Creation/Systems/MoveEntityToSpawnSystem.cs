using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Components;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation
{
  public class MoveEntityToSpawnSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly EcsEntities _convertedEntities;

    public MoveEntityToSpawnSystem(IEcsUniverse universe)
    {
      _universe = universe;

      _convertedEntities = _universe
        .FilterGame<OnConverted>()
        .Inc<Spawned>()
        .Inc<ViewRef>()
        .Collect();
    }
  
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper converted in _convertedEntities)
      {
        ref Spawned spawned = ref converted.Get<Spawned>();
        if (spawned.Spawn.TryUnpackEntity(_universe.Game, out EcsEntityWrapper spawn))
        {
          Transform transform = converted.Get<ViewRef>().View.transform;
          Transform spawnTransform = spawn.Get<ViewRef>().View.transform;
          transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
        }
      }
    }
  }
}