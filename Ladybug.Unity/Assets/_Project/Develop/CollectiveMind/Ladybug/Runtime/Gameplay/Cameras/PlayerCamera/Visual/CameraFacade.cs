using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera
{
  public class CameraFacade : IFacade, IBindable
  {
    private EntityVisual _visual;
    private EcsEntityWrapper _entity => _visual.Entity;

    public void CalculateCameraData()
    {
      CalculateLocalDeepBounds();
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.Get<EntityVisualRef>().Visual;
    }

    private void CalculateLocalDeepBounds()
    {
      if (_entity.IsAlive())
      {
        Camera camera = _entity.Get<CameraRef>().Camera;
        Transform transform = _entity.Get<TransformRef>().Transform;

        var point = new Vector3(0, 0, Mathf.Abs(transform.position.y));
        Vector3 min = camera.ViewportToWorldPoint(point);
        
        point.x = 1;
        point.y = 1;
        Vector3 max = camera.ViewportToWorldPoint(point);
        
        var bounds = Rect.MinMaxRect(min.x, min.z, max.x, max.z);
        _entity.Replace((ref CameraData cameraData) => cameraData.WorldXZBounds = bounds);
      }
    }
  }
}