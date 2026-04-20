using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONST)]
  public struct SignalTag : IEcsComponent
  {
  }
}