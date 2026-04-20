using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Utils;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  public class SignalSpawner : IGameStep
  {
    private readonly EntityFactory _entityFactory;
    private readonly SignalConfig _config;
    private readonly CameraService _cameraService;
    private readonly EcsEntities _notTrackedObstacles;

    public SignalSpawner(IEcsUniverse ecsUniverse,
      EntityFactory entityFactory,
      SignalConfig config,
      CameraService cameraService)
    {
      _entityFactory = entityFactory;
      _config = config;
      _cameraService = cameraService;

      _notTrackedObstacles = ecsUniverse
        .FilterGame<ObstacleTag>()
        .Inc<EntityVisualRef>()
        .Inc<SignalAssigment>()
        .Exc<TrackedBySignal>()
        .Exc<InCameraView>()
        .Collect();
    }
    
    public void Step()
    {
      Rect cameraBounds = _cameraService.GetCameraBounds();
        
      foreach (EcsEntityWrapper obstacle in _notTrackedObstacles)
      {
        SignalType signalType = obstacle.Get<SignalAssigment>().SignalType;

        if (signalType == SignalType.None)
        {
          continue;
        }

        SignalData signalData = _config.GetSignalData(signalType);
        
        if (signalData == null)
        {
          continue;
        }
        
        Vector3 position = obstacle.Get<TransformRef>().Transform.position;
        var positionXZ = new Vector2(position.x, position.z);

        if (positionXZ.y < cameraBounds.yMin)
        {
          continue;
        }

        float distance = MathUtils.DistanceFromPointToRect(cameraBounds, positionXZ);

        if (distance > signalData.MaxDistance)
        {
          continue;
        }

        var initContext = new SignalInitContext { Type = signalType, TrackedEntity = obstacle.PackWithWorld() };
        _entityFactory.CreateEntity(EntityType.Signal, initContext);
        obstacle.Add<TrackedBySignal>();
      }
    }
  }
}