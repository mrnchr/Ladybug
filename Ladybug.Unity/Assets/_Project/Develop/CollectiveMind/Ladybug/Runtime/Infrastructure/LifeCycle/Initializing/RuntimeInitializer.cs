using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle
{
  public class RuntimeInitializer : IRuntimeInitializer, IInitializable
  {
    private readonly List<IInitializable> _initializables = new List<IInitializable>();
    private readonly List<IInitializable> _pendingInitializables = new List<IInitializable>();
    private readonly InitializableManager _initializer;

    public bool WasInitialized { get; private set; }

    private Starter _starter;

    public RuntimeInitializer(InitializableManager initializer)
    {
      _initializer = initializer;
      _initializer.Add(this, -9999);
    }

    public void Add(IInitializable initializable)
    {
      if (!WasInitialized)
      {
        _initializer.Add(initializable);
      }
      else
      {
        if (!_starter)
        {
          _starter = new GameObject("Starter").AddComponent<Starter>();
          _starter.OnStarted += InitializeExplicit;
        }

        _initializables.Add(initializable);
      }
    }

    public void Initialize()
    {
      WasInitialized = true;
    }

    private void InitializeExplicit()
    {
      Object.Destroy(_starter.gameObject);
      
      _pendingInitializables.AddRange(_initializables);
      _initializables.Clear();
      foreach (var initializable in _pendingInitializables)
        initializable.Initialize();

      _pendingInitializables.Clear();
    }
  }
}