using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + "Base Enemy", fileName = nameof(EnemyConfig))]
  public class EnemyConfig : ScriptableObject
  {
    [Title("General")]
    [LabelText("Enemy Pause Time")]
    public float CollisionIdleTime = 2f;

    [LabelText("Animation Multiplier")]
    public float AnimationSpeedMultiplier = 1f;

    public virtual List<Type> States { get; } = new()
    {
        typeof(IdleEnemyState)
    };
  }
}