using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Tape
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + "Tape", fileName = nameof(TapeConfig))]
  public class TapeConfig : ScriptableObject
  {
    [LabelText("Tape Speed")]
    public float Speed = 7;
  }
}