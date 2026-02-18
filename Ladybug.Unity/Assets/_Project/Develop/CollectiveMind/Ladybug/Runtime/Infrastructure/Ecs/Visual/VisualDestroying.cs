using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.TEMPORARY)]
  public struct VisualDestroying : IEcsComponent
  {
  }
}