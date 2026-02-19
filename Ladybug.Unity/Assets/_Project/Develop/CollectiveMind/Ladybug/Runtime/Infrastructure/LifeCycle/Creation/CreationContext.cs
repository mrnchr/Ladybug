using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public class CreationContext
  {
    public EntityType EntityType;
    public CreationRecipe Recipe;
    public EcsEntityWrapper Entity;
    public EntityVisual Visual;
    public IFacade Facade;
    public EntityInitContext InitContext;

    public void Reset(
      EntityType entityType,
      EcsEntityWrapper entity = null,
      IFacade facade = null,
      EntityVisual visual = null,
      EntityInitContext initContext = null)
    {
      Clear();
      EntityType = entityType;
      Entity = entity;
      Facade = facade;
      Visual = visual;
      InitContext = initContext;
    }

    public void Clear()
    {
      EntityType = EntityType.None;
      Recipe = null;
      Entity = null;
      Visual = null;
      Facade = null;
      InitContext = null;
    }

    public CreationContext SetEntity(EcsEntityWrapper entity)
    {
      Entity = entity;
      return this;
    }

    public CreationContext SetFacade(IFacade facade)
    {
      Facade = facade;
      return this;
    }

    public CreationContext SetVisual(EntityVisual visual)
    {
      Visual = visual;
      return this;
    }

    public CreationContext SetInitContext(EntityInitContext initContext)
    {
      InitContext = initContext;
      return this;
    }
  }
}