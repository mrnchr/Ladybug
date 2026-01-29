namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public interface IFacadePool
  {
    TFacade GetFacade<TFacade>() where TFacade : class, IFacade;
    void DisposeFacade<TFacade>(TFacade facade) where TFacade : class, IFacade;
  }
}