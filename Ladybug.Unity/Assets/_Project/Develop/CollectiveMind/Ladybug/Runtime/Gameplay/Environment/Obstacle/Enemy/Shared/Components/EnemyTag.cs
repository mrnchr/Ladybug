using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONST)]
  public struct EnemyTag : IEcsComponent
  {
  }
}