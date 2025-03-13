using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct Cleanable : IEcsComponent
  {
  }
}