using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Unity.Cinemachine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct VirtualCameraRef : IEcsComponent
  {
    public CinemachineCamera Camera;
  }
}