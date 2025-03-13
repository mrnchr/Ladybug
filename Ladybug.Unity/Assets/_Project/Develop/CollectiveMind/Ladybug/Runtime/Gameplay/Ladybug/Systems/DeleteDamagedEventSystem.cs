using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class DeleteDamagedEventSystem : DeleteSystem<DamagedEvent>
  {
    protected DeleteDamagedEventSystem(GameWorldWrapper gameWorldWrapper) 
      : base(gameWorldWrapper, x => x.Inc<LadybugTag>())
    {
    }
  }
}