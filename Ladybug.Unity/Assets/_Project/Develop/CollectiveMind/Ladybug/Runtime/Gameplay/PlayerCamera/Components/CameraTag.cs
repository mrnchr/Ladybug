using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.PlayerCamera
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONST)]
  public struct CameraTag : IEcsComponent
  {
  }
}