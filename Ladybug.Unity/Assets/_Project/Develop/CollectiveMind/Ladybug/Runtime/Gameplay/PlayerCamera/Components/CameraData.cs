using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.PlayerCamera
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct CameraData : IEcsComponent
  {
    public Rect WorldDeepBounds;
  }
}