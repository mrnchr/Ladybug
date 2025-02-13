using Cinemachine;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera
{
  public class SetFollowTargetToCameraSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly EcsEntities _convertedCameras;
    private readonly EcsEntities _ladybugs;

    public SetFollowTargetToCameraSystem(IEcsUniverse universe)
    {
      _universe = universe;

      _ladybugs = _universe
        .FilterGame<LadybugTag>()
        .Inc<TransformRef>()
        .Collect();

      _convertedCameras = _universe
        .FilterGame<VirtualCameraTag>()
        .Exc<Targeted>()
        .Collect();
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      foreach (EcsEntityWrapper camera in _convertedCameras)
      {
        Transform targetTransform = ladybug.Get<TransformRef>().Transform;
        CinemachineVirtualCamera virtualCamera = camera.Get<VirtualCameraRef>().Camera;
        virtualCamera.Follow = targetTransform;
        camera.Add<Targeted>();
      }
    }
  }
}