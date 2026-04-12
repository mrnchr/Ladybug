using DG.Tweening;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Scissors
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + nameof(ScissorsConfig), fileName = nameof(ScissorsConfig))]
  public class ScissorsConfig : ScriptableObject
  {
    [Title("General")]
    [LabelText("Max Opening Angle")]
    public float OpenedAngle = 100f;
    [Space]
    [LabelText("Opening Animation Time")]
    public float OpenAnimationDuration = 0.5f;
    [Space]
    [LabelText("Scissors Animation Speed")]
    public float AnimationSpeed = 1f;
    [Space]
    [LabelText("Scissors Open Animation Ease")]
    public Ease OpenAnimationEase = Ease.InCubic;
    [LabelText("Scissors Close Animation Ease")]
    public Ease CloseAnimationEase = Ease.InCubic;
    
    [Title("First Mode")]
    [LabelText("Scissors Activation Distance")]
    public float ActivationDistance = 3f;
    
    [Title("Second Mode")]
    [LabelText("Scissors On Time")]
    public float OpenedStateDuration = 0.5f;
    [LabelText("Scissors Off Time")]
    public float ClosedStateDuration = 0.5f;
    [Space]
    [LabelText("Closing Animation Time")]
    public float CloseAnimationDuration = 0.5f;
  }
}
