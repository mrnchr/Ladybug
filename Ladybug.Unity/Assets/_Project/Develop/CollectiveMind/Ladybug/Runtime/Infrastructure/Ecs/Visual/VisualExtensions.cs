using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public static class VisualExtensions
  {
    public static TFacade GetFacade<TFacade>(this EcsEntityWrapper entityWrapper) where TFacade : IFacade
    {
      return (TFacade)entityWrapper.Get<FacadeRef>().Facade;
    }
    
    public static TVisual GetVisual<TVisual>(this EcsEntityWrapper entityWrapper) where TVisual : EntityVisual
    {
      return (TVisual)entityWrapper.Get<EntityVisualRef>().Visual;
    }
  }
}