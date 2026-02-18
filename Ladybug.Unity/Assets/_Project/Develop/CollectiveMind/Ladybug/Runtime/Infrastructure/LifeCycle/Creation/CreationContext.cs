using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public class CreationContext
  {
    public EntityType EntityType;
    public CreationRecipe CreationRecipe;
    public EcsEntityWrapper Entity;
    public EntityVisual Visual;
    public IFacade Facade;

    public void Clear()
    {
      EntityType = EntityType.None;
      CreationRecipe = null;
      Entity = null;
      Visual = null;
      Facade = null;
    }
  }
}