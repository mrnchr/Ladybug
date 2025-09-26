using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  public class CamerasFixedFeature : EcsFeature
  {
    public CamerasFixedFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CameraTargetFixedFeature>());
    }
  }
}