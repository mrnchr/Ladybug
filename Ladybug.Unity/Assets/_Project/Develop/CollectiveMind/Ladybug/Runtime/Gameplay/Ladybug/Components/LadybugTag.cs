using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONST)]
  public struct LadybugTag : IEcsComponent
  {
  }
}