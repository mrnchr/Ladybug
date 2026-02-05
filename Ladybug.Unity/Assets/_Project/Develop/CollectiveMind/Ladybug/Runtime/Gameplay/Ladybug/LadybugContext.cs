using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugContext
  {
    public LadybugVisual Visual;
    public EcsEntityWrapper Entity => Visual.Entity;
  }
}