using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  [Serializable]
  public struct EntityId : IEcsComponent
  {
    public EntityType Id;
  }
}