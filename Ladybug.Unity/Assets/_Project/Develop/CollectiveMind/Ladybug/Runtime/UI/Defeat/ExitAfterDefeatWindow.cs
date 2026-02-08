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

    private GameSessionController _gameSessionController;

    [Inject]
    public void Construct(GameSessionController gameSessionController)
    {
      _gameSessionController = gameSessionController;

      _reviveButton.AddListener(Revive);
      _exitButton.AddListener(AskToExit);
    }

    private void Revive()
    {
      _gameSessionController.Revive().Forget();
    }

    private void AskToExit()
    {
      _gameSessionController.SwitchToMenu().Forget();
    }

    private void OnDestroy()
    {
      _reviveButton.RemoveListener(Revive);
      _exitButton.RemoveListener(AskToExit);
    }
  }
}