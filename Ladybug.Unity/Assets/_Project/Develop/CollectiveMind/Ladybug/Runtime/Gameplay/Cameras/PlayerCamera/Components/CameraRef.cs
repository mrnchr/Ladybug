using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct CameraRef : IEcsComponent
  {
    public Camera Camera;
  }
}