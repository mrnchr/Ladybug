﻿using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Editor.Monitoring.World;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Zenject;
using Object = UnityEngine.Object;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Universe
{
  public class EcsUniversePresenter : IEcsUniversePresenter, IInitializable, ITickable, IDisposable
  {
    private readonly List<IEcsWorldWrapper> _wrappers;
    private readonly IEcsUniverseViewFactory _viewFactory;
    private readonly IEcsWorldPresenterFactory _worldFactory;
    private readonly List<IEcsWorldPresenter> _children = new List<IEcsWorldPresenter>();

    public List<IEcsWorldPresenter> Children => _children;
    public EcsUniverseView View { get; private set; }

    public bool WasInitialized { get; private set; }

    public EcsUniversePresenter(List<IEcsWorldWrapper> wrappers,
      IEcsUniverseViewFactory viewFactory,
      IEcsWorldPresenterFactory worldFactory)
    {
      _wrappers = wrappers;
      _viewFactory = viewFactory;
      _worldFactory = worldFactory;
    }

    public void Initialize()
    {
      View = _viewFactory.Create();

      foreach (IEcsWorldWrapper wrapper in _wrappers)
      {
        IEcsWorldPresenter instance = _worldFactory.Create(wrapper, this);
        instance.Initialize();
        _children.Add(instance);
        View.Worlds.Add(instance.View);
      }

      WasInitialized = true;
    }

    public void Tick()
    {
      foreach (IEcsWorldPresenter child in _children)
      {
#if UNITY_EDITOR && !DISABLE_PROFILING
        using (new Unity.Profiling.ProfilerMarker($"{child.View.gameObject.name}.Tick()").Auto())
#endif
        {
          child.Tick();
        }
      }
    }

    public void Dispose()
    {
      if (View)
        Object.Destroy(View.gameObject);
    }
  }
}