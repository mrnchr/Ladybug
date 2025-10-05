using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera
{
  public class SetFollowTargetToCameraSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly EcsEntities _convertedCameras;
    private readonly EcsEntities _cameraTargets;

    public SetFollowTargetToCameraSystem(IEcsUniverse universe)
    {
      _universe = universe;

      _cameraTargets = _universe
        .FilterGame<CameraTargetTag>()
        .Inc<TransformRef>()
        .Collect();

      _convertedCameras = _universe
        .FilterGame<VirtualCameraTag>()
        .Exc<Targeted>()
        .Collect();
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper target in _cameraTargets)
      foreach (EcsEntityWrapper camera in _convertedCameras)
      {
        Transform targetTransform = target.Get<TransformRef>().Transform;
        CinemachineCamera virtualCamera = camera.Get<VirtualCameraRef>().Camera;
        virtualCamera.Follow = targetTransform;
        camera.Add<Targeted>();
      }
    }
  }
}