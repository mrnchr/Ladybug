using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.SceneTransition;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.Pause
{
  public class ExitToMenuWindow : BaseWindow
  {
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _backButton;
    private IWindowManager _windowManager;
    private IGameSwitcher _gameSwitcher;

    [Inject]
    public void Construct(IWindowManager windowManager, IGameSwitcher gameSwitcher)
    {
      _gameSwitcher = gameSwitcher;
      _windowManager = windowManager;
      
      _exitButton.AddListener(ExitToMenu);
      _backButton.AddListener(CloseWindow);
    }

    private async void ExitToMenu()
    {
      await _windowManager.CloseWindowsBy<PauseWindow>();
      _gameSwitcher.SwitchToMenu().Forget();
    }

    private void CloseWindow()
    {
      _windowManager.CloseWindow<ExitToMenuWindow>();
    }

    private void OnDestroy()
    {
      _exitButton.RemoveListener(ExitToMenu);
      _backButton.RemoveListener(CloseWindow);
    }
  }
}