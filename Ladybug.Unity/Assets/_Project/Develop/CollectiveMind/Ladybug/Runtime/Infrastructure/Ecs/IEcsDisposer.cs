using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsDisposer : IDisposable
  {
    List<IEcsWorldWrapper> Wrappers { get; }
    List<EcsSystems> Systems { get; }
  }
}