using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class UpdateFeature : EcsFeature
  {
    public UpdateFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CollisionFeature>());
      Add(systems.Create<CamerasFeature>());
      Add(systems.Create<LadybugFeature>());
      Add(systems.Create<EnvironmentFeature>());
    }
  }
}