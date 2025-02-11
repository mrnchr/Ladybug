using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.TEMPORARY)]
  public struct OnConverted : IEcsComponent
  {
  }
}