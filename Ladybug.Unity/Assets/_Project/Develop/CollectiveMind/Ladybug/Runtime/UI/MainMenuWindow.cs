using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.UI.HowToPlay;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI
{
  public class MainMenuWindow : BaseWindow
  {
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _howToPlayButton;
    
    private IWindowManager _windowManager;
    private GameSessionController _gameSessionController;
    private Canvas _canvas;

    [Inject]
    public void Construct(IWindowManager windowManager, GameSessionController gameSessionController)
    {
      _gameSessionController = gameSessionController;
      _windowManager = windowManager;
    }

    private void Awake()
    {
      _canvas = GetComponentInParent<Canvas>(true);
      
      _playButton.AddListener(StartGame);
      _settingsButton.AddListener(OpenSettings);
      _howToPlayButton.AddListener(OpenHowToPlayWindow);
    }

    private void OnDestroy()
    {
      _playButton.RemoveListener(StartGame);
      _settingsButton.RemoveListener(OpenSettings);
      _howToPlayButton.RemoveListener(OpenHowToPlayWindow);
    }

    private void OpenSettings()
    {
      _windowManager.OpenWindow<SettingsWindow>().Forget();
    }

    private void StartGame()
    {
      _gameSessionController.SwitchToGame().Forget();
    }

    private void OpenHowToPlayWindow()
    {
      _windowManager.OpenWindow<HowToPlayWindow>().Forget();
    }

    protected override UniTask OnOpened()
    {
      _canvas.gameObject.SetActive(true);
      return UniTask.CompletedTask;
    }

    protected override UniTask OnClosed()
    {
      _canvas.gameObject.SetActive(false);
      return UniTask.CompletedTask;
    }
  }
}