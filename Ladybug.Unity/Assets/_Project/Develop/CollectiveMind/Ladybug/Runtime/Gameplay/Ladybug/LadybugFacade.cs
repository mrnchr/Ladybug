using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Utils;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugFacade : IFacade, IGameStep, IBindable
  {
    public Transform Transform => _visual.transform;
    public Vector3 Velocity { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsMoving => _isMoving;
    public ReadOnlyReactiveProperty<float> Opacity => _invincibilityHandler.Opacity;

    private readonly LadybugConfig _config;
    private readonly GameSessionData _sessionData;
    private readonly LineDrawer _lineDrawer;
    private readonly CameraService _cameraService;
    private readonly LadybugContext _context;
    private readonly ILadybugRotator _rotator;
    private readonly LadybugInvincibilityHandler _invincibilityHandler;
    private readonly LadybugBooster _booster;
    private readonly ReactiveProperty<bool> _isMoving = new ReactiveProperty<bool>();

    private LadybugVisual _visual => _context.Visual;
    private EcsEntityWrapper _entity => _context.Entity;

    public LadybugFacade(LadybugConfig config,
      GameSessionData sessionData,
      IInstantiator instantiator,
      LineDrawer lineDrawer,
      CameraService cameraService)
    {
      _config = config;
      _sessionData = sessionData;
      _lineDrawer = lineDrawer;
      _cameraService = cameraService;

      _context = instantiator.Instantiate<LadybugContext>();
      _rotator = instantiator.Instantiate<LadybugRotator>(new object[] { this });
      _invincibilityHandler = instantiator.Instantiate<LadybugInvincibilityHandler>(new object[] { _context });
      _booster = instantiator.Instantiate<LadybugBooster>(new object[] { this, _context });
    }

    public float GetScrollSpeed()
    {
      float commonSpeed = _config.Speed * _sessionData.SpeedRate.Value;
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
      Velocity = direction * GetCurrentSpeed();
      _isMoving.Value = Velocity != Vector3.zero;
    }

    public void TurnOnInvincibility()
    {
      _invincibilityHandler.HandleInvincibility();
    }

    public void Boost()
    {
      if(_sessionData.Health.Value > 0)
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
    
    public void Bind(EcsEntityWrapper entity)
    {
      _context.Visual = entity.GetVisual<LadybugVisual>();
      _booster.Initialize();
    }

    public void Step()
    {
      _booster.Step();
    }

    private float GetCurrentSpeed()
    {
      float boostMultiplier = _entity.Has<Boosting>() ? _booster.BoostMultiplier : 1;
      float currentSpeed = GetScrollSpeed() * boostMultiplier;
      return currentSpeed;
    }
  }
}