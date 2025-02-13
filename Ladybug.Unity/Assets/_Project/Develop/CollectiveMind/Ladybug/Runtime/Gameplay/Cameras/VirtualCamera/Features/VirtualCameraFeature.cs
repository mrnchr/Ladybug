using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera
{
  public class VirtualCameraFeature : EcsFeature
  {
    public VirtualCameraFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<SetFollowTargetToCameraSystem>());
    }
  }
}