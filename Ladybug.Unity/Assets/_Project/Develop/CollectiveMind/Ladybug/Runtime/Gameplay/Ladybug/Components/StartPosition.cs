using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct StartPosition : IEcsComponent
  {
    public Vector3 Position;
  }
}