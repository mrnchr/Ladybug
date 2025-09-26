using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  public class CamerasFeature : EcsFeature
  {
    public CamerasFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CameraTargetFeature>());
      Add(systems.Create<VirtualCameraFeature>());
      Add(systems.Create<CameraFeature>());
    }
  }
}