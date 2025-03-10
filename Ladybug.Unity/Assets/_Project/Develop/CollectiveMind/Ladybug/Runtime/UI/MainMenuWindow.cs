using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.SceneTransition;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI
{
  public class MainMenuWindow : WindowBase
  {
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    
    private IWindowManager _windowManager;
    private IGameSwitcher _gameSwitcher;

    [Inject]
    public void Construct(IWindowManager windowManager, IGameSwitcher gameSwitcher)
    {
      _gameSwitcher = gameSwitcher;
      _windowManager = windowManager;
    }

    private void Awake()
    {
      _playButton.AddListener(StartGame);
      _settingsButton.AddListener(OpenSettings);
    }

    private void OpenSettings()
    {
      _windowManager.OpenWindow<SettingsWindow>();
    }

    private void StartGame()
    {
      _gameSwitcher.SwitchToGame();
    }

    private void OnDestroy()
    {
      _playButton.RemoveListener(StartGame);
      _settingsButton.RemoveListener(OpenSettings);
    }
  }
}