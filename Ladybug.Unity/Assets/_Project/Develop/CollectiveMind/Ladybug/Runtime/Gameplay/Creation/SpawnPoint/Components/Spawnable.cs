using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Components
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct Spawnable : IEcsComponent
  {
  }
}