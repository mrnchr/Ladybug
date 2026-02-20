using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct YawDeviationEnabled : IEcsComponent
  {
  }
}