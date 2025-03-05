using CollectiveMind.Ladybug.Runtime.SceneTransition;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class BootInitializer : IInitializable
  {
    private readonly IGameSwitcher _gameSwitcher;

    public BootInitializer(IGameSwitcher gameSwitcher)
    {
      _gameSwitcher = gameSwitcher;
    }
    
    public void Initialize()
    {
      _gameSwitcher.SwitchToMenu();
    }
  }
}