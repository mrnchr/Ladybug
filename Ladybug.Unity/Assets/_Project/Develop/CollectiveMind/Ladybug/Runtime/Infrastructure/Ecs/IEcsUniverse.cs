﻿using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsUniverse
  {
    IEcsWorldWrapper GameWrapper { get; }
    IEcsWorldWrapper MessageWrapper { get; }
    EcsWorld Game { get; }
    EcsWorld Message { get; }
    EcsWorld.Mask FilterGame<TComponent>() where TComponent : struct;
    EcsWorld.Mask FilterMessage<TComponent>() where TComponent : struct;
  }
}