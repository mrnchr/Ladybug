using UnityEngine;
using UnityEngine.UI;

namespace CollectiveMind.Ladybug.Runtime.UI
{
  public class AdaptiveCanvasScaler : MonoBehaviour
  {
    [SerializeField] 
    [Range(0, 1)]
    private float _widthScaleFactor;
        
    [SerializeField]
    [Range(0, 1)]
    private float _heightScaleFactor;
        
    private CanvasScaler _canvasScaler;

    private void Awake()
    {
      _canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    private void LateUpdate()
    {
      var currentResolution = new Vector2(Screen.width, Screen.height);
      Vector2 referenceResolution = _canvasScaler.referenceResolution;

      if (referenceResolution == currentResolution)
      {
        return;
      }
            
      float referenceRatio = referenceResolution.x / referenceResolution.y;
      float currentRatio = currentResolution.x / currentResolution.y;
      float scaleFactor = _canvasScaler.matchWidthOrHeight;

      if (currentRatio < referenceRatio && _widthScaleFactor > 0)
      {
        scaleFactor = 1 - _widthScaleFactor;
      }

      if (currentRatio > referenceRatio && _heightScaleFactor > 0)
      {
        scaleFactor = _heightScaleFactor;
      }
            
      _canvasScaler.matchWidthOrHeight = scaleFactor;
    }
  }
}