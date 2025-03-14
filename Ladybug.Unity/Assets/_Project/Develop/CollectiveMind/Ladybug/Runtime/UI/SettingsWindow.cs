using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI
{
  public class SettingsWindow : BaseWindow
  {
    [SerializeField] private Button _closeButton;
    private IWindowManager _windowManager;

    [Inject]
    public void Construct(IWindowManager windowManager)
    {
      _windowManager = windowManager;
      _closeButton.AddListener(CloseWindow);
    }

    private void CloseWindow()
    {
      _windowManager.CloseWindow<SettingsWindow>();
    }

    private void OnDestroy()
    {
      _closeButton.RemoveListener(CloseWindow);
    }
  }
}