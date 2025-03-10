using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.UI.HUD;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class LevelInitializer : IInitializable
  {
    private readonly IWindowManager _windowManager;

    public LevelInitializer(IWindowManager windowManager)
    {
      _windowManager = windowManager;
    }
    
    public void Initialize()
    {
      _windowManager.OpenWindow<HUDWindow>();
    }
  }
}