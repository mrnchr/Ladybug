using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONST)]
  public struct VirtualCameraTag : IEcsComponent
  {
  }
}