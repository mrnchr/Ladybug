using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public class WindowInitializer
  {
    private readonly IWindowManager _windowManager;

    public WindowInitializer(IWindowManager windowManager)
    {
      _windowManager = windowManager;
    }

    public void Initialize()
    {
      BaseWindow[] windows =
        Object.FindObjectsByType<BaseWindow>(FindObjectsInactive.Include, FindObjectsSortMode.None);
      foreach (BaseWindow window in windows)
      {
        _windowManager.AddWindow(window);
      }
    }
  }
}