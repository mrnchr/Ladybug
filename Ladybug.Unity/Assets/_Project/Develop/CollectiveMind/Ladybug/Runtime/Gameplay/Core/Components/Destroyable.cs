using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct Destroyable : IEcsComponent
  {
  }
}