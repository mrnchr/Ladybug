using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Systems;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Features
{
  public class SpawnPointFeature : EcsFeature
  {
    public SpawnPointFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<SpawnSystem>());
    }
  }
}