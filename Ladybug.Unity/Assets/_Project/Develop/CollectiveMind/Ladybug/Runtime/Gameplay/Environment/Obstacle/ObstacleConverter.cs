using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using JetBrains.Annotations;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [UsedImplicitly]
  public class ObstacleConverter : ISerializedEcsConverter
  {
    [SerializeField]
    private EntityType _entityId;
    
    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity
        .Add((ref EntityId entityId) => entityId.Id = _entityId)
        .Add<ObstacleTag>()
        .Add<Destroyable>()
        .Add<Cleanable>();
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<EntityId>();
    }
  }
}