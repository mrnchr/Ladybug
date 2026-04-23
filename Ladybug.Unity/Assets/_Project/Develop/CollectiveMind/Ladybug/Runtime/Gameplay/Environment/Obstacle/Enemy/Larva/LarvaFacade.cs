using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy.Larva
{
  public class LarvaFacade : EnemyFacade
  {
    protected new LarvaConfig Config => (LarvaConfig)base.Config;

  }
}