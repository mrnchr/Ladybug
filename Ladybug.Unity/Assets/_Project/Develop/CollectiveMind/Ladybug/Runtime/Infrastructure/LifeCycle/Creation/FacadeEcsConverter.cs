using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public class FacadeEcsConverter
  {
    public void AttachFacade(IFacade facade, EcsEntityWrapper entity)
    {
      if (facade == null)
      {
        return;
      }

      entity.Add((ref FacadeRef facadeRef) => facadeRef.Facade = facade);
    }

    public void DetachFacade(EcsEntityWrapper entity)
    {
      entity.Del<FacadeRef>();
    }
  }
}