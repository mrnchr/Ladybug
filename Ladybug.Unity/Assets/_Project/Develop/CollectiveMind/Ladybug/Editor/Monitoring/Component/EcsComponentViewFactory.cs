﻿using System;
using Leopotam.EcsLite;
using Zenject;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Component
{
  public class EcsComponentViewFactory : IEcsComponentViewFactory
  {
    private readonly IInstantiator _instantiator;
    private readonly Type _genericType;

    public EcsComponentViewFactory(IInstantiator instantiator)
    {
      _instantiator = instantiator;
      _genericType = typeof(EcsComponentView<>);
    }

    public IEcsComponentView Create(Type componentType, int entity, IEcsPool pool)
    {
      Type type = _genericType.MakeGenericType(componentType);
      var instance = (IEcsComponentView) _instantiator.Instantiate(type);
      instance.SetPool(pool);
      instance.Entity = entity;
      return instance;
    }
  }
}