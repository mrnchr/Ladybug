using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.TEMPORARY)]
  public struct OneSideCollision : IEcsComponent
  {
    public CollisionType Type;
    public PackedCollider Sender;
    public Collider Other;

    public OneSideCollision(CollisionType type, PackedCollider sender, Collider other)
    {
      Type = type;
      Sender = sender;
      Other = other;
    }
  }
}