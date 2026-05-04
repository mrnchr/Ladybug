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
    private RectTransform _canvasTransform;

    public void Initialize()
    {
      var canvas = GetComponentInParent<Canvas>();
      _canvasTransform = canvas.transform as RectTransform;
      
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

    public void SetArrowPosition(Vector2 viewportPosition, Vector2 direction)
    {
      Vector3 canvasScale = _canvasTransform.localScale;
      var signalScale = new Vector3(1 / canvasScale.x, 1 / canvasScale.y, 1 / canvasScale.z);
      _signalTransform.localScale = signalScale;
      
      float distance = _arrowTransform.anchoredPosition.magnitude;
      float scaledDistance = Vector2.Scale(_arrowTransform.anchoredPosition, signalScale).magnitude;
      Vector2 arrowRectSize = _arrowTransform.rect.size;
      float arrowHalfSize = (direction.y > 0 ? arrowRectSize.y * signalScale.y : arrowRectSize.x * signalScale.x) / 2;
      Rect canvasRect = _canvasTransform.rect;
      Vector2 canvasPosition = Vector2.Scale(viewportPosition, canvasRect.size);

      if (direction.y > 0)
      {
        canvasPosition.y -= arrowHalfSize + scaledDistance;
      }
      else if (direction.x < 0)
      {
        canvasPosition.x += arrowHalfSize + scaledDistance;
      }
      else
      {
        canvasPosition.x -= arrowHalfSize + scaledDistance;
      }
      
      Vector2 signalHalfSize = Vector2.Scale(_signalTransform.rect.size, signalScale) / 2;
      var signalBox = new Rect(0, 0, canvasRect.width, canvasRect.height);
      signalBox.min += signalHalfSize;
      signalBox.max -= signalHalfSize;
      canvasPosition.x = Mathf.Clamp(canvasPosition.x, signalBox.min.x, signalBox.max.x);
      canvasPosition.y = Mathf.Clamp(canvasPosition.y, signalBox.min.y, signalBox.max.y);
      
      _signalTransform.anchoredPosition = canvasPosition;
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