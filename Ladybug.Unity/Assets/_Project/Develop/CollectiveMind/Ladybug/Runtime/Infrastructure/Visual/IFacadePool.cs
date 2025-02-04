using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public interface IFacadePool
  {
    TFacade GetFacade<TFacade>() where TFacade : class, IFacade;
  }
}