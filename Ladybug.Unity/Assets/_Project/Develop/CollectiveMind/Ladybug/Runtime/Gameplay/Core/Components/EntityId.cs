using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC + 1)]
  public struct EntityId : IEcsComponent
  {
    public EntityType Id;
  }
}