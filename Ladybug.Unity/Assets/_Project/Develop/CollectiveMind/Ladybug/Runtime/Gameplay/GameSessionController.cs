using CollectiveMind.Ladybug.Runtime.Advertisement;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Components;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.UI;
using CollectiveMind.Ladybug.Runtime.UI.Defeat;
using CollectiveMind.Ladybug.Runtime.UI.Pause;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public class GameSessionController
  {
    private readonly IWindowManager _windowManager;
    private readonly SessionService _sessionService;
    private readonly ObstacleSpawner _obstacleSpawner;
    private readonly GameplayUpdater _gameplayUpdater;
    private readonly LineDrawer _lineDrawer;
    private readonly IEcsUniverse _ecsUniverse;
    private readonly IAdService _adSvc;
    private readonly OutOfViewObserver _outOfViewObserver;
    private readonly CameraShakeController _cameraShakeController;
    private readonly EcsEntities _destroyableEntities;
    private readonly EcsEntities _cameraTargets;
    private readonly EcsEntities _virtualCameras;
    private readonly EcsEntities _ladybugs;
    private readonly EcsEntities _spawnablePoints;

    public GameSessionController(IWindowManager windowManager,
      SessionService sessionService,
      ObstacleSpawner obstacleSpawner,
      GameplayUpdater gameplayUpdater,
      LineDrawer lineDrawer,
      IEcsUniverse ecsUniverse,
      IAdService adSvc,
      OutOfViewObserver outOfViewObserver,
      CameraShakeController cameraShakeController)
    {
      _windowManager = windowManager;
      _sessionService = sessionService;
      _obstacleSpawner = obstacleSpawner;
      _gameplayUpdater = gameplayUpdater;
      _lineDrawer = lineDrawer;
      _ecsUniverse = ecsUniverse;
      _adSvc = adSvc;
      _outOfViewObserver = outOfViewObserver;
      _cameraShakeController = cameraShakeController;

      _destroyableEntities = ecsUniverse
        .FilterGame<Destroyable>()
        .Collect();

      _cameraTargets = _ecsUniverse
        .FilterGame<CameraTargetTag>()
        .Inc<RigidbodyRef>()
        .Inc<TransformRef>()
        .Collect();
      
      _virtualCameras = _ecsUniverse
        .FilterGame<VirtualCameraRef>()
        .Collect();

      _ladybugs = _ecsUniverse
        .FilterGame<LadybugTag>()
        .Inc<FacadeRef>()
        .Collect();

      _spawnablePoints = _ecsUniverse
        .FilterGame<SpawnPointTag>()
        .Inc<Spawnable>()
        .Collect();
    }

    public async UniTask SwitchToMenu()
    {
      PauseGame();
      _obstacleSpawner.StopSpawn();
      foreach (EcsEntityWrapper destroyable in _destroyableEntities)
      {
        if (destroyable.Has<Spawned>() && destroyable.Get<Spawned>().Spawn
          .TryUnpackEntity(_ecsUniverse.Game, out EcsEntityWrapper spawnPoint))
        {
          spawnPoint.Add<Spawnable>();
        }

        if(destroyable.Has<GameObjectRef>())
          Object.Destroy(destroyable.Get<GameObjectRef>().GameObject);
                
        destroyable.DelEntity();
      }
      
      foreach (EcsEntityWrapper cameraTarget in _cameraTargets)
      {
        cameraTarget.Get<TransformRef>().Transform.position = Vector3.zero;
        cameraTarget.Get<RigidbodyRef>().Rigidbody.linearVelocity = Vector3.zero;
      }

      foreach (EcsEntityWrapper virtualCamera in _virtualCameras)
      {
        CinemachineCamera camera = virtualCamera.Get<VirtualCameraRef>().Camera;
        camera.ForceCameraPosition(Vector3.zero, camera.transform.rotation);
      }

      Time.timeScale = 1;
      await _windowManager.OpenWindowAsRoot<MainMenuWindow>();
    }

    public async UniTask SwitchToGame()
    {
      _sessionService.Initialize();

      foreach (EcsEntityWrapper spawnPoint in _spawnablePoints)
      {
        spawnPoint.GetFacade<SpawnPointFacade>().Spawn();
      }
      
      _obstacleSpawner.StartSpawn();
      _outOfViewObserver.Start();
      _gameplayUpdater.SetActive(true);
      
      await _windowManager.OpenWindowAsRoot<UnpauseWindow>();
    }

    public void PauseGame()
    {
      Time.timeScale = 0;
      _gameplayUpdater.SetActive(false);
    }

    public void ResumeGame()
    {
      Time.timeScale = 1;
      _gameplayUpdater.SetActive(true);
    }

    public async UniTask Defeat()
    {
      PauseGame();
      _outOfViewObserver.Stop();
      _cameraShakeController.StopShaking();
      _lineDrawer.StopDrawing();
      await UniTask.WaitForSeconds(1, true);
      await _windowManager.OpenWindow<DefeatWindow>();
    }

    public async UniTask Revive()
    {
      await _adSvc.ShowAd();
      _sessionService.ResetHealth();
      await _windowManager.OpenWindowAsRoot<UnpauseWindow>();
      
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      {
        var facade = ladybug.GetFacade<LadybugFacade>();
        facade.OnRevived();
      }
      
      _outOfViewObserver.Start();
      
      ResumeGame();
    }
  }
}