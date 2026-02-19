using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class ObstacleTransformInitializer : IEntityInitializer
  {
    public void Initialize(CreationContext creationContext)
    {
      EcsEntityWrapper entity = creationContext.Entity;
      
      if (entity.IsAlive() && entity.Has<ObstacleTag>() && creationContext.Visual)
      {
        var initContext = creationContext.InitContext.Get<ObstacleInitContext>();
        Quaternion rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
        creationContext.Visual.transform.SetPositionAndRotation(initContext.SpawnPosition, rotation);
      }
    }
  }
}