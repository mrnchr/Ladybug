using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint
{
  public class SpawnPointVisual : EntityVisual
  {
    [field: SerializeField]
    public EntityType SpawnedEntity { get; private set; }
  }
}