using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class DetectInCameraViewSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _ecsUniverse;
    private readonly CameraService _cameraService;
    private readonly EcsEntities _inCameraViewObservers;

    public DetectInCameraViewSystem(IEcsUniverse ecsUniverse, CameraService cameraService)
    {
      _ecsUniverse = ecsUniverse;
      _cameraService = cameraService;

      _inCameraViewObservers = _ecsUniverse
        .FilterGame<CameraViewTracked>()
        .Collect();
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper observer in _inCameraViewObservers)
      {
        Vector3 position = observer.Get<CameraViewProbeRef>().CameraViewProbe.position;

        bool inCameraView = observer.Has<InCameraView>();
        bool contains = _cameraService.IsInCameraView(position);

        observer.Has<InCameraView>(contains);

        if (!inCameraView && contains)
        {
          _ecsUniverse.Publish<InCameraView>(observer);
        }
      }
    }
  }
}