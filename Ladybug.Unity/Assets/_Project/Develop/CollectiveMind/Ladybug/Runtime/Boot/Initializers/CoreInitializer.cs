using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class CoreInitializer : IInitializable
  {
    private readonly GameSwitcher _gameSwitcher;
    private readonly GameplayUpdater _gameplayUpdater;
    private readonly LineDrawer _lineDrawer;
    private readonly EcsEngine _ecsEngine;
    private readonly SessionService _sessionService;
    private readonly CameraShaker _cameraShaker;
    private readonly WindowInitializer _windowInitializer;
    private readonly IWindowManager _windowManager;

    public CoreInitializer(GameSwitcher gameSwitcher,
      GameplayUpdater gameplayUpdater,
      LineDrawer lineDrawer,
      EcsEngine ecsEngine,
      SessionService sessionService,
      CameraShaker cameraShaker,
      WindowInitializer windowInitializer)
    {
      _gameSwitcher = gameSwitcher;
      _gameplayUpdater = gameplayUpdater;
      _lineDrawer = lineDrawer;
      _ecsEngine = ecsEngine;
      _sessionService = sessionService;
      _cameraShaker = cameraShaker;
      _windowInitializer = windowInitializer;
    }
    
    public void Initialize()
    {
      _gameplayUpdater.Add(_lineDrawer);
      _gameplayUpdater.Add(_ecsEngine);
      _gameplayUpdater.Add(_sessionService);
      _gameplayUpdater.Add(_cameraShaker);

      _windowInitializer.Initialize();
      _ecsEngine.Initialize();
      _gameSwitcher.SwitchToMenu().Forget();
    }
  }
}