using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Laser
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + "Laser", fileName = nameof(LaserConfig))]
  public class LaserConfig : ScriptableObject
  {
    [LabelText("Laser Off Time")]
    public float InactiveTime = 1f;
    [LabelText("Laser On Time")]
    public float ActiveTime = 1f;
    [LabelText("Laser Color")]
    public Color Color = new(0f, 1f, 0f, 0.5f);
  }
}