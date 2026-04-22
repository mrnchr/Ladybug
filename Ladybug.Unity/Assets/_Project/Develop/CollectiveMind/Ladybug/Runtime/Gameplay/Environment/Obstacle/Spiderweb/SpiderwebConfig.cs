using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Spiderweb
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + nameof(SpiderwebConfig), fileName = nameof(SpiderwebConfig))]
  public class SpiderwebConfig : ScriptableObject
  {
    [LabelText("Spiderweb Speed Multiplier")]
    [Range(0f, 1f)]
    public float SpeedMultiplier = 0.6f;
  }
}