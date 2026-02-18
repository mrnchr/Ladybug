using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Components;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint
{
  public class SpawnPointFacade : IFacade, IBindable
  {
    private readonly EntityFactory _entityFactory;
    private readonly EcsEntityWrapper _spawnedEntity = new EcsEntityWrapper();
    
    private SpawnPointVisual _visual;
    private EcsEntityWrapper _entity => _visual.Entity;

    public SpawnPointFacade(EntityFactory entityFactory)
    {
      _entityFactory = entityFactory;
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<SpawnPointVisual>();
    }

    public void Spawn()
    {
      _entityFactory.CreateEntity(_visual.SpawnedEntity, _spawnedEntity);
      _spawnedEntity.Add((ref Spawned spawned) => spawned.Spawn = _entity.PackedEntity);
      Transform transform = _spawnedEntity.Get<TransformRef>().Transform;
      transform.SetPositionAndRotation(_visual.transform.position, _visual.transform.rotation);
    }
  }
}