using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class CameraTargetFeature : EcsFeature
  {
    public CameraTargetFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CalculateScoreSystem>());
    }
  }
}