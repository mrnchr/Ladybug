using CollectiveMind.Ladybug.Runtime.Gameplay.Creation;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class UpdateFeature : EcsFeature
  {
    public UpdateFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CreationFeature>());
    }    
  }
}