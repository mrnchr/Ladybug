using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Scissors
{
  public class ScissorsVisual : EntityVisual
  {
    private const float ANIMATION_TRANSITION_DURATION = 0.1f;
    private static readonly int _openAnimaitonHash = Animator.StringToHash("Open");
    private static readonly int _closeAnimaitonHash = Animator.StringToHash("Close");
    
    private Animator _animator;

    public Animator Animator => _animator;
    
    private void Awake()
    {
      _animator = GetComponentInChildren<Animator>();
    }

    public void SetAnimationSpeed(float speed)
    {
      if (speed <= 0f)
        speed = 0.1f;
      
      _animator.speed = speed;
    }

    public void PlayOpenAnimation(float transitionDuration = ANIMATION_TRANSITION_DURATION)
    {
      _animator.CrossFade(_openAnimaitonHash, transitionDuration);
    }

    public void PlayCloseAnimation(float transitionDuration = ANIMATION_TRANSITION_DURATION)
    {
      _animator.CrossFade(_closeAnimaitonHash, transitionDuration);
    }

    public void PlayAnimation(string animationName, float transitionDuration = ANIMATION_TRANSITION_DURATION)
    {
      _animator.CrossFade(animationName, transitionDuration);
    }
  }
}