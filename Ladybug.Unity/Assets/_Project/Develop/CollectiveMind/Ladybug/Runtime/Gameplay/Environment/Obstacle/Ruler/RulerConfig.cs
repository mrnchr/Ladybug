using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Ruler
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + "Ruler", fileName = nameof(RulerConfig))]
  public class RulerConfig : ScriptableObject
  {
    public float RulerSpawnAngle;
  }
}