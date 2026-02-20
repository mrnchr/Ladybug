using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Systems;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment
{
  public class EnvironmentFeature : EcsFeature
  {
    public EnvironmentFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CanvasFeature>());
      Add(systems.Create<ObstacleFeature>());
      Add(systems.Create<CleanSpawnablesSystem>());      
    }
  }
}