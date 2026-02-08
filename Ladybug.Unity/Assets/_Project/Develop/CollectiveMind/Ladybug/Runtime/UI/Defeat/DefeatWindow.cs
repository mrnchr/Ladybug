using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.Defeat
{
  public class DefeatWindow : BaseWindow
  {
    [SerializeField]
    private Button _reviveButton;

    [SerializeField]
    private Button _exitButton;

    private IWindowManager _windowManager;
    private GameSessionController _gameSessionController;

    [Inject]
    public void Construct(IWindowManager windowManager, GameSessionController pauseSessionController)
    {
      _gameSessionController = pauseSessionController;
      _windowManager = windowManager;

      _reviveButton.AddListener(Revive);
      _exitButton.AddListener(AskToExit);
    }

    private void Revive()
    {
      _gameSessionController.Revive().Forget();
    }

    private void AskToExit()
    {
      _windowManager.OpenWindow<ExitAfterDefeatWindow>();
    }

    private void OnDestroy()
    {
      _reviveButton.RemoveListener(Revive);
      _exitButton.RemoveListener(AskToExit);
    }
  }
}