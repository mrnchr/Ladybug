using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera
{
  public class CameraFacade : IFacade
  {
    private EcsEntityWrapper _entityWrapper;

    public void SetVisual(EcsEntityWrapper entityWrapper)
    {
      _entityWrapper = entityWrapper;
    }

    public void CalculateCameraData()
    {
      CalculateLocalDeepBounds();
    }

    private void CalculateLocalDeepBounds()
    {
      if (_entityWrapper.IsAlive())
      {
        Camera camera = _entityWrapper.Get<CameraRef>().Camera;
        Transform transform = _entityWrapper.Get<TransformRef>().Transform;

        var point = new Vector3(0, 0, Mathf.Abs(transform.position.y));
        Vector3 min = camera.ViewportToWorldPoint(point);
        
        point.x = 1;
        point.y = 1;
        Vector3 max = camera.ViewportToWorldPoint(point);
        
        var bounds = Rect.MinMaxRect(min.x, min.z, max.x, max.z);
        _entityWrapper.Replace((ref CameraData cameraData) => cameraData.WorldXZBounds = bounds);
      }
    }
  }
}