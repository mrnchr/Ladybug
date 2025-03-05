using CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
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
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(IWindowManager windowManager, ISceneLoader sceneLoader)
    {
      _sceneLoader = sceneLoader;
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
      _sceneLoader.LoadAsync(SceneType.Game);
    }

    private void OnDestroy()
    {
      _playButton.RemoveListener(StartGame);
      _settingsButton.RemoveListener(OpenSettings);
    }
  }
}