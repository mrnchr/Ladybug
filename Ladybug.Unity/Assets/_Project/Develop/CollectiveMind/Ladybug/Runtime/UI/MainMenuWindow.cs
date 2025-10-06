using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
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
    
    private IWindowManager _windowManager;
    private GameSwitcher _gameSwitcher;
    private Canvas _canvas;

    [Inject]
    public void Construct(IWindowManager windowManager, GameSwitcher gameSwitcher)
    {
      _gameSwitcher = gameSwitcher;
      _windowManager = windowManager;
    }

    private void Awake()
    {
      _canvas = GetComponentInParent<Canvas>(true);
      
      _playButton.AddListener(StartGame);
      _settingsButton.AddListener(OpenSettings);
    }

    private void OnDestroy()
    {
      _playButton.RemoveListener(StartGame);
      _settingsButton.RemoveListener(OpenSettings);
    }

    private void OpenSettings()
    {
      _windowManager.OpenWindow<SettingsWindow>().Forget();
    }

    private void StartGame()
    {
      _gameSwitcher.SwitchToGame().Forget();
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