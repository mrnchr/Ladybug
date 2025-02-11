using System;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Component
{
  public interface IEcsComponentViewFactory
  {
    IEcsComponentView Create(Type componentType, int entity, IEcsPool pool);
  }
}