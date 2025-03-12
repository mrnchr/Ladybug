using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class CollisionFixedFeature : EcsFeature
  {
    public CollisionFixedFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<DeleteCollisionSystem>());
      Add(systems.Create<PackFixedCollisionsSystem>());
    }
  }
}