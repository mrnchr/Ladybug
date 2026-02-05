using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugFacade : IFacade, IGameStep
  {
    public Transform Transform => _visual.transform;
    public Vector3 Velocity { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsMoving => _isMoving;
    public ReadOnlyReactiveProperty<float> Opacity => _invincibilityHandler.Opacity;

    private readonly LadybugConfig _config;
    private readonly GameSessionData _sessionData;
    private readonly GameSwitcher _gameSwitcher;
    private readonly LineDrawer _lineDrawer;
    private readonly LadybugContext _context;
    private readonly ILadybugRotator _rotator;
    private readonly LadybugInvincibilityHandler _invincibilityHandler;
    private readonly LadybugBooster _booster;
    private readonly ReactiveProperty<bool> _isMoving = new ReactiveProperty<bool>();

    private LadybugVisual _visual => _context.Visual;
    private EcsEntityWrapper _entity => _context.Entity;

    public LadybugFacade(LadybugConfig config,
      GameSessionData sessionData,
      GameSwitcher gameSwitcher,
      IInstantiator instantiator, 
      LineDrawer lineDrawer)
    {
      _config = config;
      _sessionData = sessionData;
      _gameSwitcher = gameSwitcher;
      _lineDrawer = lineDrawer;

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

    public void Boost()
    {
      _booster.Boost();
    }

    public void DrawLine(Vector3 startPoint, Vector3 endPoint, float width, Color color)
    {
      _lineDrawer.DrawLine(startPoint, endPoint, width, color);
    }

    public void Bind(LadybugVisual visual)
    {
      _context.Visual = visual;
      _sessionData.Health
        .Pairwise()
        .Where(pair => pair.Current < pair.Previous)
        .Select(pair => pair.Current)
        .Subscribe(OnHealthChanged)
        .AddTo(_visual);
      
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

    private void OnHealthChanged(int health)
    {
      if (health <= 0)
      {
        _gameSwitcher.Defeat().Forget();
        return;
      }

      _invincibilityHandler.HandleInvincibility();
    }
  }
}