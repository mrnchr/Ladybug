using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  public class SignalVisual : EntityVisual
  {
    [SerializeField]
    private RectTransform _signalTransform;

    [SerializeField]
    private Image _markImage;
    
    [SerializeField]
    private RectTransform _arrowTransform;
    
    [SerializeField]
    private Image _arrowImage;
    
    private readonly List<Graphic> _graphics = new List<Graphic>();

    public void Initialize()
    {
      SignalData signalData = Entity.Get<SignalContext>().Data;
      List<Color> colors = signalData.Colors;

      if (colors.Count == 0)
      {
        return;
      }
      
      _graphics.Add(_markImage);
      _graphics.Add(_arrowImage);
      
      SetColor(_graphics, signalData.Colors[0]);
      
      if (colors.Count == 1 || signalData.ColorChangingTime <= 0f)
      {
        return;
      }
      
      float segmentDuration = signalData.ColorChangingTime / colors.Count / 2;
      Sequence sequence = DOTween.Sequence();

      for (int i = 0; i < colors.Count; i++)
      {
        Color fromColor = colors[i];
        Color toColor = colors[(i + 1) % colors.Count];

        sequence.Append(
          DOVirtual.Color(
              fromColor,
              toColor,
              segmentDuration,
              color => SetColor(_graphics, color))
            .SetEase(Ease.Linear));
      }

      sequence
        .SetLoops(-1, LoopType.Restart)
        .SetLink(gameObject);
    }

    public void SetArrowPosition(Vector2 screenPosition, Vector2 direction)
    {
      float distance = _arrowTransform.anchoredPosition.magnitude;
      float halfSize = (direction.y > 0 ? _arrowTransform.sizeDelta.y : _arrowTransform.sizeDelta.x) / 2;
      Vector2 position = screenPosition;

      if (direction.y > 0)
      {
        position.y -= halfSize + distance;
      }
      else if (direction.x < 0)
      {
        position.x += halfSize + distance;
      }
      else
      {
        position.x -= halfSize + distance;
      }
      
      _signalTransform.anchoredPosition = position;
      _arrowTransform.anchoredPosition = direction * distance;
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      _arrowTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    private void SetColor(IReadOnlyList<Graphic> graphics, Color color)
    {
      foreach (Graphic graphic in graphics)
      {
        graphic.color = color;
      }
    }
  }
}