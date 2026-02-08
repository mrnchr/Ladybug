using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
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
    private GameSessionController _gameSessionController;

    [Inject]
    public void Construct(IWindowManager windowManager, GameSessionController gameSessionController)
    {
      _gameSessionController = gameSessionController;
      _windowManager = windowManager;
      
      _exitButton.AddListener(ExitToMenu);
      _backButton.AddListener(CloseWindow);
    }

    private void ExitToMenu()
    {
      _gameSessionController.SwitchToMenu().Forget();
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