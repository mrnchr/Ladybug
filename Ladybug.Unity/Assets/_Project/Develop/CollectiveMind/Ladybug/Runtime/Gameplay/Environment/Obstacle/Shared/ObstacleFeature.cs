using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class ObstacleFeature : EcsFeature
  {
    public ObstacleFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<DetectInCameraViewSystem>());
    }
  }
}