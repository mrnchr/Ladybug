using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Utils;
using Leopotam.EcsLite;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugFacade : IFacade, IGameStep, IBindable, IInitializable, IDisposable
  {
    public Transform Transform => _visual.transform;
    public Vector3 Velocity { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsMoving => _isMoving;
    public ReadOnlyReactiveProperty<float> Opacity => _invincibilityHandler.Opacity;
    public ReadOnlyReactiveProperty<bool> IsInvincible => _invincibilityHandler.IsInvincible;

    private readonly LadybugConfig _config;
    private readonly SessionService _session;
    private readonly LineDrawer _lineDrawer;
    private readonly CameraService _cameraService;
    private readonly LadybugContext _context;
    private readonly ILadybugRotator _rotator;
    private readonly LadybugInvincibilityHandler _invincibilityHandler;
    private readonly LadybugBooster _booster;
    private readonly SpiderwebSlowdownHandler _spiderwebSlowdownHandler;
    private readonly ReactiveProperty<bool> _isMoving = new ReactiveProperty<bool>();
    
    private DisposableBag _disposableBag;
    private Vector3 _direction;

    private LadybugVisual _visual => _context.Visual;
    private EcsEntityWrapper _entity => _context.Entity;

    public LadybugFacade(LadybugConfig config,
      SessionService session,
      IInstantiator instantiator,
      LineDrawer lineDrawer,
      CameraService cameraService)
    {
      _config = config;
      _session = session;
      _lineDrawer = lineDrawer;
      _cameraService = cameraService;

      _context = instantiator.Instantiate<LadybugContext>();
      _rotator = instantiator.Instantiate<LadybugRotator>(new object[] { this });
      _invincibilityHandler = instantiator.Instantiate<LadybugInvincibilityHandler>(new object[] { _context });
      _booster = instantiator.Instantiate<LadybugBooster>(new object[] { this, _context });
      _spiderwebSlowdownHandler = instantiator.Instantiate<SpiderwebSlowdownHandler>(new object[] { this });
      
      _disposableBag.Add(_spiderwebSlowdownHandler);
    }

    public float GetScrollSpeed()
    {
      float commonSpeed = _config.Speed * _session.SpeedRate.Value;
      return commonSpeed;
    }

    public void CheckBounds()
    {
      if (!_entity.Has<Boosting>())
      {
        _rotator.CheckBounds();
      }
    }

    public void UpdateVelocity(Vector3 direction)
    {
      _direction = direction;
      Velocity = direction * (_session.Speed.CurrentValue * _session.SpeedRate.Value);
      _isMoving.Value = Velocity != Vector3.zero;
    }

    public void TurnOnInvincibility()
    {
      _invincibilityHandler.HandleInvincibility();
    }

    public void Boost()
    {
      if(_session.Health.CurrentValue > 0)
      {
        _booster.Boost();
      }
    }

    public void DrawLine(Vector3 startPoint, Vector3 endPoint, float width, Color color)
    {
      _lineDrawer.DrawLine(startPoint, endPoint, width, color);
    }

    public void OnRevived()
    {
      Transform.forward = Vector3.forward;
      _invincibilityHandler.HandleInvincibility();
      
      if (_cameraService.IsEntityOutsideCamera(_entity))
      {
        Rect cameraBounds = _cameraService.GetCameraBounds();
        Transform.SetPosition(Axis.X, Axis.Z, cameraBounds.center);
      }
    }

    public void EnterSpiderweb(EcsPackedEntity packedEntity)
    {
      _spiderwebSlowdownHandler.EnterSpiderweb(packedEntity);
    }

    public void LeaveSpiderweb(EcsPackedEntity packedEntity)
    {
      _spiderwebSlowdownHandler.LeaveSpiderweb(packedEntity);
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _context.Visual = entity.GetVisual<LadybugVisual>();
      _booster.Initialize();
    }
    
    public void Initialize()
    {
      _direction = Vector2.zero;
      _disposableBag.Add(_session.Speed.Subscribe(_ => UpdateVelocity(_direction)));
    }

    public void Step()
    {
      _booster.Step();
      _spiderwebSlowdownHandler.Step();
    }

    public void Dispose()
    {
      _disposableBag.Dispose();
    }
  }
}