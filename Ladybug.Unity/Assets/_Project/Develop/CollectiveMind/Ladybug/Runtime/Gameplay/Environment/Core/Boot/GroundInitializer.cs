using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Boot
{
  public class GroundInitializer : IEntityInitializer
  {
    public void InitializeEntity(CreationContext creationContext)
    {
      EcsEntityWrapper entity = creationContext.Entity;
      
      if (entity.IsAlive() && entity.Has<GroundPointRef>() && creationContext.Visual)
      {
        Transform groundChecker = entity.Get<GroundPointRef>().GroundPoint;
        creationContext.Visual.transform.position -= groundChecker.localPosition;
      }
    }
  }
}