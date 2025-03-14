using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONST)]
  public struct CameraTargetTag : IEcsComponent
  {
  }
}