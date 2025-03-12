using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.TEMPORARY)]
  public struct CollisionMessage : IEcsComponent
  {
  }
}