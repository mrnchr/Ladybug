﻿using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONST)]
  public struct ObstacleTag : IEcsComponent
  {
  }
}