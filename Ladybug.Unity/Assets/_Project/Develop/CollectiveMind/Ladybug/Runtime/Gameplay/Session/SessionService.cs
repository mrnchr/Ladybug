using CollectiveMind.Ladybug.Runtime.Configuration;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Session
{
  public class SessionService : IInitializable, IFixedTickable
  {
    private readonly GameSessionData _sessionData;
    private readonly GameSessionConfig _config;

    private float _lastRaiseScore;

    public SessionService(GameSessionData sessionData, IConfigProvider configProvider)
    {
      _sessionData = sessionData;
      _config = configProvider.Get<GameSessionConfig>();
    }
    
    public void Initialize()
    {
      _sessionData.Health.Value = _config.HealthPoints;
      _sessionData.RevivalCount.Value = _config.RevivalCount;
    }

    public void ResetHealth()
    {
      _sessionData.Health.Value = _config.HealthPoints;
    }

    public void FixedTick()
    {
      if (_sessionData.Score.Value - _lastRaiseScore >= _config.RaiseDistance * _sessionData.SpeedRate.Value)
      {
        _lastRaiseScore = _sessionData.Score.Value;
        _sessionData.SpeedRate.Value *= _config.RaiseRate;
      }
    }
  }
}