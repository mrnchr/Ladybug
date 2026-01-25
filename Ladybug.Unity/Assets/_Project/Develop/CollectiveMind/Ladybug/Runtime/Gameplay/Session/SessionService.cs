namespace CollectiveMind.Ladybug.Runtime.Gameplay.Session
{
  public class SessionService : IGameFixedStep
  {
    private readonly GameSessionData _sessionData;
    private readonly GameSessionConfig _config;

    private float _lastRaiseScore;

    public SessionService(GameSessionData sessionData, GameSessionConfig config)
    {
      _sessionData = sessionData;
      _config = config;
    }
    
    public void Initialize()
    {
      _lastRaiseScore = 0;
      _sessionData.Health.Value = _config.HealthPoints;
      _sessionData.Score.Value = 0;
      _sessionData.RevivalCount.Value = _config.RevivalCount;
      _sessionData.SpeedRate.Value = 1;
    }

    public void ResetHealth()
    {
      _sessionData.Health.Value = _config.HealthPoints;
    }

    public void FixedStep()
    {
      if (_sessionData.Score.Value - _lastRaiseScore >= _config.RaiseDistance * _sessionData.SpeedRate.Value)
      {
        _lastRaiseScore = _sessionData.Score.Value;
        _sessionData.SpeedRate.Value *= _config.RaiseRate;
      }
    }
  }
}