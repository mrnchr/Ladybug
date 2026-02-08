using CollectiveMind.Ladybug.Runtime.Gameplay;
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
    private readonly GameSessionController _gameSessionController;
    private readonly GameplayUpdater _gameplayUpdater;
    private readonly LineDrawer _lineDrawer;
    private readonly EcsEngine _ecsEngine;
    private readonly SessionService _sessionService;
    private readonly WindowInitializer _windowInitializer;
    private readonly IWindowManager _windowManager;

    public CoreInitializer(GameSessionController gameSessionController,
      GameplayUpdater gameplayUpdater,
      LineDrawer lineDrawer,
      EcsEngine ecsEngine,
      SessionService sessionService,
      WindowInitializer windowInitializer)
    {
      _gameSessionController = gameSessionController;
      _gameplayUpdater = gameplayUpdater;
      _lineDrawer = lineDrawer;
      _ecsEngine = ecsEngine;
      _sessionService = sessionService;
      _windowInitializer = windowInitializer;
    }
    
    public void Initialize()
    {
      _gameplayUpdater.Add(_lineDrawer);
      _gameplayUpdater.Add(_ecsEngine);
      _gameplayUpdater.Add(_sessionService);

      _windowInitializer.Initialize();
      _ecsEngine.Initialize();
      _gameSessionController.SwitchToMenu().Forget();
    }
  }
}