using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera
{
  public class CameraFacade : IFacade, IBindable, IEntityInitializable
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

    public void Initialize(EntityInitContext initContext)
    {
      Camera camera = _entity.Get<CameraRef>().Camera;
      float depth = camera.transform.position.y;
      float height = 2f * depth * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
      float width = height * camera.aspect;
      Transform triggerTransform = _entity.Get<ColliderRef>().Collider.transform;
      triggerTransform.localScale = new Vector3(width, height, depth);
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