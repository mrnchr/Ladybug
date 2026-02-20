using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct CameraViewProbeRef : IEcsComponent
  {
    public Transform CameraViewProbe;
  }
}