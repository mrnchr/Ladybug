using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Scissors
{
  public class ScissorsVisual : EntityVisual
  {
    [Header("Scissors")]
    [SerializeField, Tooltip("Negative angle")] private Transform _leftBlade;
    [SerializeField, Tooltip("Positive angle")] private Transform _rightBlade;

    private ScissorsConfig _config;
    private Tween _tween;

    [Inject]
    private void Construct(ScissorsConfig config)
    {
      _config = config;
    }

    public void PlayOpenAnimation()
    {
      var positiveOpenedAngle = new Vector3(0f, _config.OpenedAngle, 0f);
      var negativeOpenedAngle = new Vector3(0f, -_config.OpenedAngle, 0f);
      var duration = _config.OpenAnimationDuration * _config.AnimationSpeed;
      
      _tween?.Kill();
      _tween = DOTween.Sequence()
        .Join(_leftBlade.DOLocalRotate(negativeOpenedAngle, duration))
        .Join(_rightBlade.DOLocalRotate(positiveOpenedAngle, duration))
        .SetEase(_config.OpenAnimationEase)
        .SetLink(gameObject)
        .Play();
    }

    public void PlayCloseAnimation()
    {
      var closedAngle = Vector3.zero;
      var duration = _config.CloseAnimationDuration;
      
      _tween?.Kill();
      _tween = DOTween.Sequence()
        .Join(_leftBlade.DOLocalRotate(closedAngle, duration))
        .Join(_rightBlade.DOLocalRotate(closedAngle, duration))
        .SetEase(_config.CloseAnimationEase)
        .SetLink(gameObject)
        .Play();
    }
  }
}