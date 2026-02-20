using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class CoreInitializer : MonoBehaviour, IInitializable
  {
    [SerializeField]
    private EntityVisual _cameraVisual;

    [SerializeField]
    private CameraTargetVisual _cameraTargetVisual;

    [SerializeField]
    private EntityVisual _virtualCameraVisual;

    [SerializeField]
    private List<SpawnPointVisual> _spawnPointVisuals;

    private CoreCreationRecipeRegistrar _creationRecipeRegistrar;
    private CoreEntityInitializerRegistrar _entityInitializerRegistrar;
    private GameSessionController _gameSessionController;
    private GameplayUpdater _gameplayUpdater;
    private LineDrawer _lineDrawer;
    private EcsEngine _ecsEngine;
    private CameraService _cameraService;
    private SessionService _sessionService;
    private WindowInitializer _windowInitializer;
    private EntityFactory _entityFactory;

    [Inject]
    private void Construct(CoreCreationRecipeRegistrar creationRecipeRegistrar,
      CoreEntityInitializerRegistrar entityInitializerRegistrar,
      GameSessionController gameSessionController,
      GameplayUpdater gameplayUpdater,
      LineDrawer lineDrawer,
      EcsEngine ecsEngine,
      CameraService cameraService,
      SessionService sessionService,
      WindowInitializer windowInitializer,
      EntityFactory entityFactory)
    {
      _creationRecipeRegistrar = creationRecipeRegistrar;
      _entityInitializerRegistrar = entityInitializerRegistrar;
      _gameSessionController = gameSessionController;
      _gameplayUpdater = gameplayUpdater;
      _lineDrawer = lineDrawer;
      _ecsEngine = ecsEngine;
      _cameraService = cameraService;
      _sessionService = sessionService;
      _windowInitializer = windowInitializer;
      _entityFactory = entityFactory;
    }

    public void Initialize()
    {
      _creationRecipeRegistrar.RegisterRecipes();
      _entityInitializerRegistrar.RegisterInitializers();
      _gameplayUpdater.Add(_lineDrawer);
      _gameplayUpdater.Add(_ecsEngine);
      _gameplayUpdater.Add(_sessionService);

      _windowInitializer.Initialize();
      _ecsEngine.Initialize();

      EcsEntityWrapper camera = _entityFactory.CreateEntityWithVisual(EntityType.Camera, _cameraVisual);
      _cameraService.Initialize(camera);
      
      _entityFactory.CreateEntityWithVisual(EntityType.VirtualCamera, _virtualCameraVisual);
      _entityFactory.CreateEntityWithVisual(EntityType.CameraTarget, _cameraTargetVisual);

      foreach (SpawnPointVisual spawnPointVisual in _spawnPointVisuals)
      {
        _entityFactory.CreateEntityWithVisual(EntityType.SpawnPoint, spawnPointVisual);
      }

      _gameSessionController.SwitchToMenu().Forget();
    }
  }
}