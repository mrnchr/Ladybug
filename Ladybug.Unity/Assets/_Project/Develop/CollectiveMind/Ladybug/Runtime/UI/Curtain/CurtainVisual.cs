using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.Curtain
{
  public class CurtainVisual : MonoBehaviour
  {
    [SerializeField]
    private Slider _slider;

    private CurtainFacade _facade;

    [Inject]
    public void Construct(CurtainFacade facade)
    {
      _facade = facade;

      _facade.IsVisible.Subscribe(ChangeVisibility);
      _facade.Progress.Subscribe(ChangeProgress);
    }

    private void ChangeProgress(float progress)
    {
      _slider.value = progress;
    }

    private void ChangeVisibility(bool isVisible)
    {
      gameObject.SetActive(isVisible);
    }
  }
}