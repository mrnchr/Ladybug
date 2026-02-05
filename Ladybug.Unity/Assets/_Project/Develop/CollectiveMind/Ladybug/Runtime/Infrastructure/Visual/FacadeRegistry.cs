using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public class FacadeRegistry : IFacadeRegistry
  {
    private readonly IInstantiator _container;
    private readonly GameplayUpdater _gameplayUpdater;
    private readonly List<IFacade> _facades = new List<IFacade>();

    public FacadeRegistry(IInstantiator container, GameplayUpdater gameplayUpdater)
    {
      _container = container;
      _gameplayUpdater = gameplayUpdater;
    }

    public TFacade GetFacade<TFacade>() where TFacade : class, IFacade
    {
      if (_facades.Find(f => f is TFacade) is not TFacade facade)
      {
        facade = CreateFacade<TFacade>();
        _facades.Add(facade);
      }

      return facade;
    }

    public void DisposeFacade<TFacade>(TFacade facade) where TFacade : class, IFacade
    {
      (facade as IDisposable)?.Dispose();

      if (facade is IGameStep gameStep)
      {
        _gameplayUpdater.Remove(gameStep);
      }

      if (facade is IGameFixedStep gameFixedStep)
      {
        _gameplayUpdater.Remove(gameFixedStep);
      }

      _facades.Remove(facade);
    }

    private TFacade CreateFacade<TFacade>() where TFacade : class, IFacade
    {
      var facade = _container.Instantiate<TFacade>();

      if (facade is IGameStep gameStep)
      {
        _gameplayUpdater.Add(gameStep);
      }

      if (facade is IGameFixedStep gameFixedStep)
      {
        _gameplayUpdater.Add(gameFixedStep);
      }

      return facade;
    }
  }
}