using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement.Boot
{
  public class WindowInitializer : IInitializable
  {
    private readonly IWindowManager _windowManager;
    private readonly WindowBase[] _windows;

    public WindowInitializer(IWindowManager windowManager)
    {
      _windowManager = windowManager;
      _windows = Object.FindObjectsByType<WindowBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }
    
    public void Initialize()
    {
      foreach (WindowBase window in _windows)
      {
        _windowManager.AddWindow(window);
      }
    }
  }
}