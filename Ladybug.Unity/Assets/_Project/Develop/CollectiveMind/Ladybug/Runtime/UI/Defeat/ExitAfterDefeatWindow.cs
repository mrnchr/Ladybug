using CollectiveMind.Ladybug.Runtime.Advertisement;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.SceneTransition;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.Defeat
{
  public class ExitAfterDefeatWindow : BaseWindow
  {
    [SerializeField]
    private Button _reviveButton;

    [SerializeField]
    private Button _exitButton;

    private Reviver _reviver;
    private IWindowManager _windowManager;
    private IAdService _adSvc;
    private IGameSwitcher _gameSwitcher;

    [Inject]
    public void Construct(Reviver reviver,
      IWindowManager windowManager,
      IAdService adSvc,
      IGameSwitcher gameSwitcher)
    {
      _reviver = reviver;
      _windowManager = windowManager;
      _adSvc = adSvc;
      _gameSwitcher = gameSwitcher;

      _reviveButton.AddListener(Revive);
      _exitButton.AddListener(AskToExit);
    }

    private async void Revive()
    {
      await _adSvc.ShowAd();
      await _windowManager.CloseWindow<DefeatWindow>();
      _reviver.Revive();
    }

    private async void AskToExit()
    {
      await _windowManager.CloseWindowsBy<DefeatWindow>();
      _gameSwitcher.SwitchToMenu();
    }

    private void OnDestroy()
    {
      _reviveButton.RemoveListener(Revive);
      _exitButton.RemoveListener(AskToExit);
    }
  }
}