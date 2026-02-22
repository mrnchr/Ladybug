using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class ObstacleYawDeviationInitializer : IEntityInitializer 
  {
    private readonly CameraService _cameraService;

    public ObstacleYawDeviationInitializer(CameraService cameraService)
    {
      _cameraService = cameraService;
    }
    
    public void InitializeEntity(CreationContext creationContext)
    {
      EcsEntityWrapper entity = creationContext.Entity;
      
      if (_cameraService.Camera.IsAlive() && entity.IsAlive() && entity.Has<YawDeviationEnabled>())
      {
        Vector3 cameraPosition = _cameraService.Camera.Get<TransformRef>().Transform.position;
        Vector3 entityPosition = entity.Get<TransformRef>().Transform.position;
        Vector3 direction = (cameraPosition - entityPosition).normalized;
        direction.y = 0;
        
        Quaternion look = Quaternion.LookRotation(direction);

        float angleRange = entity.Get<AngularDeviation>().MaxAngle;
        float offsetAngle = Random.Range(-angleRange, angleRange);
        Quaternion rotation = look * Quaternion.AngleAxis(offsetAngle, Vector3.up);
        entity.Get<TransformRef>().Transform.rotation = rotation;
      }
    }
  }
}