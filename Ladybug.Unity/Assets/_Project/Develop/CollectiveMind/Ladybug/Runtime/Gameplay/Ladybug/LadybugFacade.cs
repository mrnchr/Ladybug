using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugFacade : IFacade
  {
    private readonly ILadybugRotator _rotator;
    private readonly GameSessionData _sessionData;
    private readonly GameSwitcher _gameSwitcher;
    private readonly LadybugConfig _config;

    private LadybugVisual _visual;

    public Transform Transform => _visual.transform;
    public Vector3 Velocity { get; private set; }
    public ReactiveProperty<bool> IsMoving { get; }

    public LadybugFacade(IConfigProvider configProvider,
      ILadybugRotator rotator,
      GameSessionData sessionData,
      GameSwitcher gameSwitcher)
    {
      _rotator = rotator;
      _sessionData = sessionData;
      _gameSwitcher = gameSwitcher;
      _config = configProvider.Get<LadybugConfig>();

      IsMoving = new ReactiveProperty<bool>(false);
      _sessionData.Health.Subscribe(CheckToDie);
    }

    private void CheckToDie(int health)
    {
      if (health <= 0)
        _gameSwitcher.Defeat().Forget();
    }

    public void SetVisual(LadybugVisual visual)
    {
      _visual = visual;
    }

    public void UpdateVelocity(Vector3 direction)
    {
      Velocity = direction * (_config.Speed * _sessionData.SpeedRate.Value);
      IsMoving.Value = Velocity != Vector3.zero;
    }

    public void CheckBound()
    {
      _rotator.CheckBound();
    }
  }
}