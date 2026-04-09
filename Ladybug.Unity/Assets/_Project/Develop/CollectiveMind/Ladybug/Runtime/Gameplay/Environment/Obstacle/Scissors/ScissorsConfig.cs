using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Scissors
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + nameof(ScissorsConfig), fileName = nameof(ScissorsConfig))]
  public class ScissorsConfig : ScriptableObject
  {
    [Title("General")]
    [LabelText("Scissors Animation Speed")]
    public float AnimationSpeed = 1f;
    
    [Title("First Mode")]
    [LabelText("Scissors Activation Distance")]
    public float ActivationDistance = 3f;
    
    [Title("Second Mode")]
    [LabelText("Scissors On Time")]
    public float OpenedDuration = 0.5f;
    [LabelText("Scissors Off Time")]
    public float ClosedDuration = 0.5f;
  }
}