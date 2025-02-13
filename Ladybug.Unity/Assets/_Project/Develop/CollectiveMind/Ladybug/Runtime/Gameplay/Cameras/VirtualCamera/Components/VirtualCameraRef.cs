using System;
using Cinemachine;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct VirtualCameraRef : IEcsComponent
  {
    public CinemachineVirtualCamera Camera;
  }
}