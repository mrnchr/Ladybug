using System;
using DG.Tweening;
using DG.Tweening.Core;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  public class CameraShakeController : IDisposable
  {
    public ReadOnlyReactiveProperty<Color> ShakingColor { get; }
    public ReadOnlyReactiveProperty<Vector2> ShakingOffset { get; }
    
    private readonly CameraConfig _config;
    
    private readonly ReactiveProperty<Color> _shakingColor = new ReactiveProperty<Color>();
    private readonly ReactiveProperty<Vector2> _shakingOffset = new ReactiveProperty<Vector2>();
    
    private readonly DOGetter<Color> _shakingColorGetter;
    private readonly DOSetter<Color> _shakingColorSetter;
    
    private readonly DOGetter<Vector2> _shakingOffsetGetter;
    private readonly DOSetter<Vector2> _shakingOffsetSetter;
    
    private Sequence _fadeSequence;
    private Sequence _offsetSequence;


    public CameraShakeController(CameraConfig config)
    {
      _config = config;

      ShakingColor = _shakingColor.ToReadOnlyReactiveProperty();
      ShakingOffset = _shakingOffset.ToReadOnlyReactiveProperty();
      
      _shakingColorGetter = () => _shakingColor.Value;
      _shakingColorSetter = x => _shakingColor.Value = x;
      
      _shakingOffsetGetter = () => _shakingOffset.Value;
      _shakingOffsetSetter = x => _shakingOffset.Value = x;
    }

    public void Dispose()
    {
      StopShaking();
    }

    public void Shake()
    {
      StopShaking();
      Fade();
      Animate();
    }

    public void StopShaking()
    {
      _fadeSequence?.Kill();
      _fadeSequence = null;
      
      _offsetSequence?.Kill();
      _offsetSequence = null;
    }

    private void Fade()
    {
      float fadeDuration = _config.ShakingDuration / 2;
      _shakingColor.Value = Color.clear;
      _fadeSequence = DOTween.Sequence()
        .Append(DOTween.To(_shakingColorGetter, _shakingColorSetter, _config.ShakingColor, fadeDuration))
        .Append(DOTween.To(_shakingColorGetter, _shakingColorSetter, Color.clear, fadeDuration))
        .Play();
    }

    private void Animate()
    {
      if (_config.Positions.Count == 0)
      {
        return;
      }

      float frameDuration = _config.ShakingDuration / _config.Positions.Count;
      _shakingOffset.Value = Vector2.zero;
      _offsetSequence = DOTween.Sequence();

      foreach (CameraKeyframe keyframe in _config.Positions)
      {
        float distance = keyframe.Offset;
        Vector2 direction = Vector2.zero;

        direction.x = keyframe.Direction switch
        {
          DirectionType.UpRight or DirectionType.Right or DirectionType.DownRight => 1,
          DirectionType.DownLeft or DirectionType.Left or DirectionType.UpLeft => -1,
          _ => direction.x
        };

        direction.y = keyframe.Direction switch
        {
          DirectionType.UpRight or DirectionType.Up or DirectionType.UpLeft => 1,
          DirectionType.DownLeft or DirectionType.Down or DirectionType.DownRight => -1,
          _ => direction.y
        };

        direction.Normalize();
        Vector2 offset = direction * distance;
        _offsetSequence.Append(DOTween.To(_shakingOffsetGetter, _shakingOffsetSetter, offset, frameDuration));
      }
      
      _offsetSequence.Play();
    }
  }
}