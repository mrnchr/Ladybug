using System;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct RigidbodyRef : IEcsComponent
  {
    public Rigidbody Rigidbody;
  }
}