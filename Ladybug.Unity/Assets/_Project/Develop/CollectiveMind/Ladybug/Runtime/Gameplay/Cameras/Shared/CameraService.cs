using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Utils;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  public class CameraService
  {
    public EcsEntityWrapper Camera { get; } = new EcsEntityWrapper();

    private readonly CameraConfig _cameraConfig;

    public CameraService(CameraConfig cameraConfig)
    {
      _cameraConfig = cameraConfig;
    }

    public void Initialize(EcsEntityWrapper camera)
    {
      Camera.Copy(camera);
    }

    public bool IsInCameraView(Vector3 position)
    {
      if (!Camera.IsAlive())
      {
        return false;
      }

      Rect cameraBounds = GetCameraBounds();
      Vector3 positionXZ = position.GetXZY();
      bool isInView = cameraBounds.Contains(positionXZ);
      return isInView;
    }

    public bool IsEntityOutsideCamera(EcsEntityWrapper entity)
    {
      if (!Camera.IsAlive())
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
      if (!Camera.IsAlive())
      {
        return new Rect();
      }

      var cameraFacade = Camera.GetFacade<CameraFacade>();
      cameraFacade.CalculateCameraData();
      Rect bounds = Camera.Get<CameraData>().WorldXZBounds;
      return bounds;
    }
  }
}