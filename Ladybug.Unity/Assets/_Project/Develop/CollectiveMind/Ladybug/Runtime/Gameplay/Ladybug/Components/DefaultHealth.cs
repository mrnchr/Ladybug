using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct DefaultHealth : IEcsComponent
  {
    public int HP;
  }
}