using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct TrackedBySignal : IEcsComponent
  {
  }
}