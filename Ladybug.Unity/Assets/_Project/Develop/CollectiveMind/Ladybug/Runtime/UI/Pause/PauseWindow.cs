using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.Pause
{
  public class PauseWindow : BaseWindow
  {
    [SerializeField]
    private Button _resumeButton;

    [SerializeField]
    private Button _exitButton;

    private IWindowManager _windowManager;
    private GameSessionController _gameSessionController;

    [Inject]
    public void Construct(GameSessionController gameSessionController, IWindowManager windowManager)
    {
      _gameSessionController = gameSessionController;
      _windowManager = windowManager;

      _resumeButton.AddListener(ResumeGame);
      _exitButton.AddListener(AskToExit);
    }

    protected override UniTask OnOpened()
    {
      _gameSessionController.PauseGame();
      return UniTask.CompletedTask;
    }

    protected override UniTask OnClosed()
    {
      _gameSessionController.ResumeGame();
      return UniTask.CompletedTask;
    }

    private void ResumeGame()
    {
      _windowManager.CloseWindow<PauseWindow>().Forget();
    }

    private void AskToExit()
    {
      _windowManager.OpenWindow<ExitToMenuWindow>();
    }

    private void OnDestroy()
    {
      _resumeButton.RemoveListener(ResumeGame);
      _exitButton.RemoveListener(AskToExit);
    }
  }
}