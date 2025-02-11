using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using TriInspector;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Components
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct Spawned : IEcsComponent
  {
    [ShowInInspector]
    public EcsPackedEntity Spawn;
  }
}