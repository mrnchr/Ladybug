using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.TEMPORARY)]
  public struct TwoSideCollision : IEcsComponent
  {
    public CollisionType Type;
    public PackedCollider Sender;
    public PackedCollider Other;

    public TwoSideCollision(CollisionType type, PackedCollider sender, PackedCollider other)
    {
      Type = type;
      Sender = sender;
      Other = other;
    }
  }
}