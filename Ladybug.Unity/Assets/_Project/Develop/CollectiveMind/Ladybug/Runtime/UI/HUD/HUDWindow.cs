using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.UI.Pause;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.HUD
{
  public class HUDWindow : WindowBase
  {
    [SerializeField]
    private Button _pauseButton;

    private IWindowManager _windowManager;

    [Inject]
    public void Construct(IWindowManager windowManager)
    {
      _windowManager = windowManager;
      
      _pauseButton.AddListener(PauseGame);
    }

    private void PauseGame()
    {
      _windowManager.OpenWindow<PauseWindow>();
    }

    private void OnDestroy()
    {
      _pauseButton.RemoveListener(PauseGame);
    }
  }
}