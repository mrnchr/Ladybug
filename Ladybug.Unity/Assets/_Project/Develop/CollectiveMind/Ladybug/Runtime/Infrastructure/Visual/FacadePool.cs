using System.Collections.Generic;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public class FacadePool : IFacadePool
  {
    private readonly DiContainer _container;
    private readonly List<IFacade> _facades = new List<IFacade>();

    public FacadePool(DiContainer container)
    {
      _container = container;
    }

    public TFacade GetFacade<TFacade>() where TFacade : class, IFacade
    {
      if (_facades.Find(f => f is TFacade) is not TFacade facade)
      {
        facade = _container.Instantiate<TFacade>();
        _facades.Add(facade);
      }

      return facade;
    }
  }
}