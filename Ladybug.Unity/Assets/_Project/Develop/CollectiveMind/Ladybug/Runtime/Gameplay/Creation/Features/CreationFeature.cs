using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Features;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation
{
  public class CreationFeature : EcsFeature
  {
    public CreationFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<SpawnPointFeature>());
      
      Add(systems.Create<DeleteSystem<OnConverted>>());
      Add(systems.Create<CreateEntityWithViewSystem>());
      
      Add(systems.Create<MoveEntityToSpawnSystem>());
    }
  }
}