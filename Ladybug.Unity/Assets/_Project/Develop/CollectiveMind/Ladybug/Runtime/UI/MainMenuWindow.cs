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
    [SerializeField] private Button _exitButton;

    [Inject]
    public void Construct(IWindowManager windowManager)
    {
      
    }

    private void Awake()
    {
      _playButton.AddListener(OnPlayClicked);
      _settingsButton.AddListener(OnSettingsClicked);
      _exitButton.AddListener(OnExitClicked);
    }


    private void OnDestroy()
    {
      _playButton.RemoveListener(OnPlayClicked);
      _settingsButton.RemoveListener(OnSettingsClicked);
      _exitButton.RemoveListener(OnExitClicked);
    }
  }
}