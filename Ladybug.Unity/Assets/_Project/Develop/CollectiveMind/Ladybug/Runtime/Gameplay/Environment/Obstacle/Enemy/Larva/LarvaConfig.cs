using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy.Larva
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + "Base Enemy", fileName = nameof(EnemyConfig))]
  public class LarvaConfig : EnemyConfig
  {
    [Title("Larva")]
    [LabelText("Enemy Speed Buff")]
    public float SpeedSubtractor = 3;
  }
}