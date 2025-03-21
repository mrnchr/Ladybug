﻿using CollectiveMind.Ladybug.Runtime.Configuration;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Session
{
  public class SessionService : IInitializable
  {
    private readonly GameSessionData _sessionData;
    private readonly GameSessionConfig _config;

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
  }
}