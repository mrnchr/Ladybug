namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public static class VisualExtensions
  {
    public static TFacade GetFacade<TFacade>(this EcsEntityWrapper entityWrapper) where TFacade : class
    {
      return (TFacade)entityWrapper.Get<VisualFacadeRef>().Facade;
    }
  }
}