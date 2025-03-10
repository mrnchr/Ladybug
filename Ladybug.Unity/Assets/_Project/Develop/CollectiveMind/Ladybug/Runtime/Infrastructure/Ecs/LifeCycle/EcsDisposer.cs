﻿using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsDisposer : IEcsDisposer
  {
    public List<IEcsWorldWrapper> Wrappers { get; } = new();
    public List<EcsSystems> Systems { get; } = new();

    public EcsDisposer(List<IEcsWorldWrapper> wrappers)
    {
      Wrappers.AddRange(wrappers);
    }

    public void Dispose()
    {
      foreach (EcsSystems systems in Systems)
        systems.Destroy();

      foreach (IEcsWorldWrapper wrapper in Wrappers)
      {
        wrapper.World.Destroy();
      }
    }
  }
}