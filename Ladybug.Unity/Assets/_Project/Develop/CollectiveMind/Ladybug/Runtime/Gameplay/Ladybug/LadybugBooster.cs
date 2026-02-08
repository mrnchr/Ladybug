using System.Threading;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugBooster
  {
    public float BoostMultiplier { get; private set; } = 1;

    private readonly LadybugFacade _facade;
    private readonly LadybugContext _context;
    private readonly CameraConfig _cameraConfig;
    private readonly EcsEntities _cameras;
    private float _lineWidth;
    private float _lineHalfLength;

    public LadybugBooster(LadybugFacade facade,
      LadybugContext context,
      CameraConfig cameraConfig,
      IEcsUniverse ecsUniverse)
    {
      _facade = facade;
      _context = context;
      _cameraConfig = cameraConfig;

      _cameras = ecsUniverse
        .FilterGame<CameraTag>()
        .Inc<CameraData>()
        .Collect();
    }

    public void Initialize()
    {
      CapsuleCollider collider = _context.Visual.Skin.CapsuleCollider;

      float radius = collider.radius;
      float height = collider.height;
      float halfCylinder = Mathf.Max(0f, height - 2f * radius) * 0.5f;

      _lineWidth = radius * 2f;
      _lineHalfLength = halfCylinder + radius;
    }

    public void Boost()
    {
      RunBoostAsync(_context.Visual.GetCancellationTokenOnDestroy()).Forget();
    }

    public void Step()
    {
      if (_context.Entity.Has<Boosting>())
      {
        Transform transform = _context.Visual.transform;
        Vector3 frontPoint = transform.position + transform.forward * _lineHalfLength;
        Vector3 backPoint = transform.position - transform.forward * _lineHalfLength;
        _facade.DrawLine(frontPoint, backPoint, _lineWidth, Color.white);
      }
    }

    private async UniTask RunBoostAsync(CancellationToken token = default(CancellationToken))
    {
      _context.Entity.Add<Boosting>();
      _facade.Transform.forward = Vector3.forward;

      float targetPositionZ = GetCameraPositionZ();
      float distance = Mathf.Abs(targetPositionZ - _facade.Transform.position.z);
      BoostMultiplier = _cameraConfig.CameraSpeedMultiplier
        + distance / (_facade.GetScrollSpeed() * _cameraConfig.DamageBoostDuration);

      while (!token.IsCancellationRequested && _facade.Transform.position.z < targetPositionZ)
      {
        await UniTask.Yield(token).SuppressCancellationThrow();
        targetPositionZ = GetCameraPositionZ();
      }

      BoostMultiplier = 1;

      if (_context.Entity.IsAlive())
      {
        _context.Entity.Del<Boosting>();
      }
    }

    private float GetCameraPositionZ()
    {
      foreach (EcsEntityWrapper camera in _cameras)
      {
        Rect bounds = camera.Get<CameraData>().WorldXZBounds;
        float targetPositionZ = Mathf.Lerp(bounds.yMin, bounds.yMax, _cameraConfig.BoostZPosition);
        return targetPositionZ;
      }

      return 0;
    }
  }
}