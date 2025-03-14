using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugFeature : EcsFeature
  {
    public LadybugFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<DeleteDamagedEventSystem>());
      Add(systems.Create<DamageLadybugSystem>());
      
      Add(systems.Create<CalculateScoreSystem>());
    }
  }
}