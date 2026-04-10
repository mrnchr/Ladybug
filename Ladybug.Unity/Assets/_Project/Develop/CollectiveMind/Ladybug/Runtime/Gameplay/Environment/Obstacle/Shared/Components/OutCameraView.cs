using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONTINUOUS)]
  public struct OutCameraView : IEcsComponent
  {
  }
}