using CollectiveMind.Ladybug.Runtime.Advertisement;
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

    private Reviver _reviver;
    private IWindowManager _windowManager;
    private IPauseSwitcher _pauseSwitcher;
    private IAdService _adSvc;

    [Inject]
    public void Construct(Reviver reviver,
      IWindowManager windowManager,
      IPauseSwitcher pauseSwitcher,
      IAdService adSvc)
    {
      _adSvc = adSvc;
      _pauseSwitcher = pauseSwitcher;
      _windowManager = windowManager;
      _reviver = reviver;

      _reviveButton.AddListener(Revive);
      _exitButton.AddListener(AskToExit);
    }

    private async void Revive()
    {
      await _adSvc.ShowAd();
      await _windowManager.CloseWindow<DefeatWindow>();
      _reviver.Revive();
    }

    private void AskToExit()
    {
      _windowManager.OpenWindow<ExitAfterDefeatWindow>();
    }

    protected override UniTask OnOpened()
    {
      _pauseSwitcher.PauseGame();
      return UniTask.CompletedTask;
    }

    protected override UniTask OnClosed()
    {
      _pauseSwitcher.ResumeGame();
      return UniTask.CompletedTask;
    }

    private void OnDestroy()
    {
      _reviveButton.RemoveListener(Revive);
      _exitButton.RemoveListener(AskToExit);
    }
  }
}