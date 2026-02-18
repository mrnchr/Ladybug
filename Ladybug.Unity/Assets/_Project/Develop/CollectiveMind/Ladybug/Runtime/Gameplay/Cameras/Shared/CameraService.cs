using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  public class CameraService
  {
    private readonly CameraConfig _cameraConfig;
    private readonly EcsEntities _cameras;

    public CameraService(IEcsUniverse universe, CameraConfig cameraConfig)
    {
      _cameraConfig = cameraConfig;

      _cameras = universe
        .FilterGame<CameraTag>()
        .Inc<CameraData>()
        .Inc<GameObjectRef>()
        .Collect();
    }
    
    public bool IsEntityOutsideCamera(EcsEntityWrapper entity)
    {
      if (!_cameras.Any())
      {
        return false;
      }
        
      Rect cameraBounds = GetCameraBounds();
      Transform transform = entity.Get<TransformRef>().Transform;
      bool isOutside = transform.position.z < cameraBounds.yMin - _cameraConfig.FrameOffset;
      return isOutside;
    }

    public Rect GetCameraBounds()
    {
      foreach (EcsEntityWrapper camera in _cameras)
      {
        var cameraFacade = camera.GetFacade<CameraFacade>();
        cameraFacade.CalculateCameraData();
        Rect bounds = camera.Get<CameraData>().WorldXZBounds;
        return bounds;
      }

      return new Rect();
    }
  }
}