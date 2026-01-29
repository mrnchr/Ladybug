using System;
using System.Collections.Generic;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public class FacadePool : IFacadePool
  {
    private readonly List<IFacade> _facades = new List<IFacade>();
    private readonly IInstantiator _container;

    public FacadePool(IInstantiator container)
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

    public void DisposeFacade<TFacade>(TFacade facade) where TFacade : class, IFacade
    {
      (facade as IDisposable)?.Dispose();
      _facades.Remove(facade);
    }
  }
}