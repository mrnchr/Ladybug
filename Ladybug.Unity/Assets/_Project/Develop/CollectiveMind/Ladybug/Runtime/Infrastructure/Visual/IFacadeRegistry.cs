namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public interface IFacadeRegistry
  {
    TFacade GetFacade<TFacade>() where TFacade : class, IFacade;
    void DisposeFacade<TFacade>(TFacade facade) where TFacade : class, IFacade;
  }
}