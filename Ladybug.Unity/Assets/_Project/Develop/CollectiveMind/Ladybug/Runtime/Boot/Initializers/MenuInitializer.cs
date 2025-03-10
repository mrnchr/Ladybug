using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class MenuInitializer : IInitializable
  {
    private readonly IWindowManager _windowManager;

    public MenuInitializer(IWindowManager windowManager)
    {
      _windowManager = windowManager;
    }
    
    public void Initialize()
    {
      _windowManager.OpenWindow<MainMenuWindow>();
    }
  }
}