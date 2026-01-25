using UnityEngine;
using Zenject;
using R3;
using UnityEngine.UI;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  public class ForegroundVisual : MonoBehaviour
  {
    [SerializeField]
    private Image _foregroundImage;
    
    [Inject]
    public void Construct(CameraShakeController cameraShakeController)
    {
      cameraShakeController.ShakingColor.Subscribe(UpdateColor).AddTo(this);
    }

    private void UpdateColor(Color color)
    {
      _foregroundImage.color = color;
    }
  }
}