using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class FixedUpdateFeature : EcsFeature
  {
    public FixedUpdateFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CollisionFixedFeature>());
    }
  }
}