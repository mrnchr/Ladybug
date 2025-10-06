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
    private GameSwitcher _gameSwitcher;

    [Inject]
    public void Construct(GameSwitcher gameSwitcher, IWindowManager windowManager)
    {
      _gameSwitcher = gameSwitcher;
      _windowManager = windowManager;

      _resumeButton.AddListener(ResumeGame);
      _exitButton.AddListener(AskToExit);
    }

    protected override UniTask OnOpened()
    {
      _gameSwitcher.PauseGame();
      return UniTask.CompletedTask;
    }

    protected override UniTask OnClosed()
    {
      _gameSwitcher.ResumeGame();
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