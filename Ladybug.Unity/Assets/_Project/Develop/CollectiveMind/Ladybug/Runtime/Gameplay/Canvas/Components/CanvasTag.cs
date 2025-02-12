using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Canvas
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.CONST)]
  public struct CanvasTag : IEcsComponent
  {
  }
}