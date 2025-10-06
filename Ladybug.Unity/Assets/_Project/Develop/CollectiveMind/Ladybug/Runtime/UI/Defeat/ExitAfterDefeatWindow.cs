using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using Cysharp.Threading.Tasks;
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

    private GameSwitcher _gameSwitcher;

    [Inject]
    public void Construct(GameSwitcher gameSwitcher)
    {
      _gameSwitcher = gameSwitcher;

      _reviveButton.AddListener(Revive);
      _exitButton.AddListener(AskToExit);
    }

    private void Revive()
    {
      _gameSwitcher.Revive().Forget();
    }

    private void AskToExit()
    {
      _gameSwitcher.SwitchToMenu().Forget();
    }

    private void OnDestroy()
    {
      _reviveButton.RemoveListener(Revive);
      _exitButton.RemoveListener(AskToExit);
    }
  }
}