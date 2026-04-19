using System;
using System.Threading;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Laser
{
  public class LaserFacade : IFacade, IBindable, IEntityInitializable, IDisposable, IGameFixedStep
  {
    private readonly LaserConfig _config;
    private readonly IEcsUniverse _universe;
    private readonly CameraService _cameraService;
    private readonly SessionService _session;
    
    private readonly EcsEntities _ladybugs;
    
    private DisposableBag _disposableBag;

    private CancellationTokenSource _cycleCts;
    private bool _isActive;
    
    private float _maxRayDistance;
    private Vector3 _rayOrigin;
    private Vector3 _rayEndPoint;
    
    private LaserVisual _visual;
    private EcsEntityWrapper _entity => _visual.Entity;

    public LaserFacade(LaserConfig config,
      IEcsUniverse universe,
      CameraService cameraService,
      SessionService session)
    {
      _universe = universe;
      _config = config;
      _cameraService = cameraService;
      _session = session;
      
      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<ColliderRef>()
        .Collect();
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<LaserVisual>();
      _disposableBag.Add(_universe.Subscribe<InCameraView>(_entity, OnCameraViewEnter));
      _disposableBag.Add(_universe.Subscribe<OutCameraView>(_entity, OnCameraViewExit));
    }
    
    public void Initialize(EntityInitContext initContext)
    {
      _maxRayDistance = _cameraService.GetCameraDiagonal() * 2f;
      _visual.SetRayColor(_config.Color);
    }

    public void FixedStep()
    {
      if (!_isActive)
        return;

      Vector3 rayOrigin = _visual.RayOrigin.position;
      Vector3 direction = _visual.RayOrigin.forward;
      Vector3 endPosition = rayOrigin + direction * _maxRayDistance;
      if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, _maxRayDistance))
      {
        endPosition = hit.point;

        foreach (EcsEntityWrapper ladybug in _ladybugs)
        {
          Collider col = ladybug.Get<ColliderRef>().Collider;
          if (col == hit.collider && !ladybug.Has<Invincible>())
          {
            _session.SubtractHealth(1);
          }
        }
      }

      _visual.SetRayEndPosition(endPosition);
    }
    
    private async UniTask RunCycle(CancellationToken token)
    {
      _isActive = false;

      while (!token.IsCancellationRequested)
      {
        _visual.ClearRayEndPosition();
        await UniTask.WaitForSeconds(_config.InactiveTime, cancellationToken: token).SuppressCancellationThrow();
        
        if (token.IsCancellationRequested)
          return;
        
        _isActive = true;
        await UniTask.WaitForSeconds(_config.ActiveTime, cancellationToken: token).SuppressCancellationThrow();
        _isActive = false;
      }
    }
    
    private void OnCameraViewEnter()
    {
      _cycleCts = _cycleCts?.CancelDisposeAndForget();
      _cycleCts = CancellationTokenSource.CreateLinkedTokenSource(_visual.destroyCancellationToken);
      RunCycle(_cycleCts.Token).Forget();
    }

    private void OnCameraViewExit()
    {
      _cycleCts = _cycleCts?.CancelDisposeAndForget();
      _visual.ClearRayEndPosition();
      _isActive = false;
    }

    public void Dispose()
    {
      _cycleCts = _cycleCts?.CancelDisposeAndForget();
      _disposableBag.Dispose();
    }
  }
}