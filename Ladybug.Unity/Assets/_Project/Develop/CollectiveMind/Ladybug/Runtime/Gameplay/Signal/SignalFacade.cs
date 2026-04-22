using System;
using System.Threading;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Infrastructure;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.UI.HUD;
using CollectiveMind.Ladybug.Runtime.Utils;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  public class SignalFacade : IFacade, IBindable, IEntityInitializable, IGameStep, IDisposable
  {
    private readonly EntityFactory _entityFactory;
    private readonly CameraService _cameraService;
    private readonly IEcsUniverse _ecsUniverse;
    private readonly SignalConfig _config;
    private readonly IWindowManager _windowManager;
    private readonly EcsEntityWrapper _cachedObstacle = new EcsEntityWrapper();
    private DisposableBag _disposableBag;

    private SignalVisual _visual;
    private EcsEntityWrapper _entity => _visual.Entity;
    private CancellationToken _visualDestroyToken;
    private CancellationTokenSource _delayDestroyTokenSource;

    public SignalFacade(EntityFactory entityFactory,
      CameraService cameraService,
      IEcsUniverse ecsUniverse,
      SignalConfig config,
      IWindowManager windowManager)
    {
      _entityFactory = entityFactory;
      _cameraService = cameraService;
      _ecsUniverse = ecsUniverse;
      _config = config;
      _windowManager = windowManager;
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<SignalVisual>();
      _visualDestroyToken = _visual.destroyCancellationToken;
    }

    public void Initialize(EntityInitContext initContext)
    {
      var signalInitContext = initContext.Get<SignalInitContext>();
      SignalData signalData = _config.GetSignalData(signalInitContext.Type);

      ref SignalContext signalContext = ref _entity.AddOrGet<SignalContext>();
      signalContext.Data = signalData;
      signalContext.TrackedEntity = signalInitContext.TrackedEntity;
      
      _cachedObstacle.Unpack(signalContext.TrackedEntity);
      _disposableBag.Add(_ecsUniverse.Subscribe<InCameraView>(_cachedObstacle, HandleEnteredCameraView));

      var hudWindow = _windowManager.GetWindow<HUDWindow>();
      hudWindow.SetSignalParent(_visual.transform);
      
      _visual.Initialize();
    }

    public void Step()
    {
      if (_entity.IsAlive())
      {
        var context = _entity.Get<SignalContext>();
        _cachedObstacle.Unpack(context.TrackedEntity);

        if (!_cachedObstacle.IsAlive())
        {
          _entityFactory.DestroyEntity(_entity);
          return;
        }

        if (_delayDestroyTokenSource != null && !_cachedObstacle.Has<InCameraView>())
        {
          CTSUtils.CancelDisposeAndClear(ref _delayDestroyTokenSource);
        }

        Vector3 obstaclePosition = _cachedObstacle.Get<TransformRef>().Transform.position;
        var obstaclePositionXZ = new Vector2(obstaclePosition.x, obstaclePosition.z);
        Rect cameraBounds = _cameraService.GetCameraBounds();

        if (obstaclePositionXZ.y < cameraBounds.yMin)
        {
          _entityFactory.DestroyEntity(_entity);
          return;
        }
        
        Vector2 closestPoint = MathUtils.FindClosestPointOnRect(cameraBounds, obstaclePositionXZ);

        if (closestPoint != obstaclePositionXZ)
        {
          float distance = Vector2.Distance(closestPoint, obstaclePositionXZ);

          if (distance > context.Data.MaxDistance)
          {
            _cachedObstacle.Has<TrackedBySignal>(false);
            _entityFactory.DestroyEntity(_entity);
            return;
          }
        }

        bool isOnLeft = Mathf.Approximately(closestPoint.x, cameraBounds.xMin);
        bool isOnRight = Mathf.Approximately(closestPoint.x, cameraBounds.xMax);

        Vector2 arrowDirection;
        
        if (!_cachedObstacle.Has<InCameraView>())
        {
          arrowDirection = isOnLeft ? Vector2.left 
            : isOnRight ? Vector2.right : Vector2.up;
        }
        else
        {
          float leftDistance = Mathf.Abs(obstaclePositionXZ.x - cameraBounds.xMin);
          float rightDistance = Mathf.Abs(obstaclePositionXZ.x - cameraBounds.xMax);
          float topDistance = Mathf.Abs(obstaclePositionXZ.y - cameraBounds.yMax);

          if (leftDistance < rightDistance && leftDistance < topDistance)
          {
            arrowDirection = Vector2.left;
          }
          else if (rightDistance < topDistance)
          {
            arrowDirection = Vector2.right;
          }
          else
          {
            arrowDirection = Vector2.up;
          }
        }

        Vector2 localVector = closestPoint - cameraBounds.min;
        var viewportPosition = new Vector2(localVector.x / cameraBounds.width, localVector.y / cameraBounds.height);
        var screenPosition = new Vector2(Screen.width * viewportPosition.x, Screen.height * viewportPosition.y);
        _visual.SetArrowPosition(screenPosition, arrowDirection);
      }
    }

    public void Dispose()
    {
      CTSUtils.CancelDisposeAndClear(ref _delayDestroyTokenSource);
      _disposableBag.Dispose();
    }

    private void HandleEnteredCameraView()
    {
      CTSUtils.CancelDisposeAndClear(ref _delayDestroyTokenSource);
      _delayDestroyTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_visualDestroyToken);
      DelayDestroyAsync(_delayDestroyTokenSource.Token).SuppressCancellationThrow().Forget();
    }

    private async UniTask DelayDestroyAsync(CancellationToken token = default(CancellationToken))
    {
      if (_entity.IsAlive())
      {
        float hideDelay = _entity.Get<SignalContext>().Data.HideDelay;
        await UniTask.WaitForSeconds(hideDelay, cancellationToken: token).SuppressCancellationThrow();
        
        if (token.IsCancellationRequested)
        {
          return;
        }
        
        var context = _entity.Get<SignalContext>();
        _cachedObstacle.Unpack(context.TrackedEntity);

        if (_cachedObstacle.IsAlive())
        {
          _cachedObstacle.Has<TrackedBySignal>(false);
        }
        
        _entityFactory.DestroyEntity(_entity);
      }
    }
  }
}