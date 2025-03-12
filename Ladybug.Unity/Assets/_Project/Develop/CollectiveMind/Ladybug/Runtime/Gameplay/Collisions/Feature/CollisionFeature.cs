using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class CollisionFeature : EcsFeature
  {
    public CollisionFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<DeleteCollisionSystem>());
      Add(systems.Create<PackCollisionsSystem>());
    }
  }
}