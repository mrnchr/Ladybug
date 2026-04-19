using System;
using System.Collections.Generic;
using System.Linq;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Session
{
  public class SessionService : IGameFixedStep
  {
    public readonly ReactiveProperty<float> Score;
    public readonly ReactiveProperty<int> RevivalCount;
    public readonly ReactiveProperty<float> SpeedRate;
    
    private readonly GameSessionConfig _config;
    private readonly LadybugConfig _ladybugConfig;
    
    private readonly ReactiveProperty<int> _health;
    private readonly ReactiveProperty<float> _speed;
    
    private readonly List<SpeedModifier> _speedModifiers;

    private float _lastRaiseScore;

    public ReadOnlyReactiveProperty<int> Health => _health;
    public ReadOnlyReactiveProperty<float> Speed => _speed;
    
    public SessionService(GameSessionConfig config, LadybugConfig ladybugConfig)
    {
      _config = config;
      _ladybugConfig = ladybugConfig;
      
      _health = new ReactiveProperty<int>();
      Score = new ReactiveProperty<float>();
      RevivalCount = new ReactiveProperty<int>();
      SpeedRate = new ReactiveProperty<float>();
      _speed = new ReactiveProperty<float>();
      
      _speedModifiers = new List<SpeedModifier>();
    }
    
    public void Initialize()
    {
      _lastRaiseScore = 0;
      
      _health.Value = _config.HealthPoints;
      Score.Value = 0f;
      RevivalCount.Value = _config.RevivalCount;
      SpeedRate.Value = 0f;
      _speed.Value = _ladybugConfig.Speed;
    }

    public void SubtractHealth(int amount)
    {
      if (amount < 0f)
        return;
      
      _health.Value = Mathf.Max(0, _health.Value - amount);
    }
    
    public void ResetHealth()
    {
      _health.Value = _config.HealthPoints;
    }

    public IDisposable AddSpeedModifier(SpeedModifier modifier)
    {
      if (modifier == null)
      {
        Debug.LogWarning($"[{nameof(SessionService)}] Trying to add null speed modifier");
        return null;
      }

      if (_speedModifiers.Contains(modifier))
      {
        Debug.LogWarning($"[{nameof(SessionService)}] Trying to add same speed modifier");
        return modifier;
      }

      _speedModifiers.Add(modifier);
      modifier.OnDispose += () =>
      {
        _speedModifiers.Remove(modifier);
        RecalculateSpeed();
      };
      
      RecalculateSpeed();
      return modifier;
    }
    
    public void FixedStep()
    {
      if (Score.Value - _lastRaiseScore >= _config.RaiseDistance * SpeedRate.Value)
      {
        _lastRaiseScore = Score.Value;
        SpeedRate.Value *= _config.RaiseRate;
      }
    }

    private void RecalculateSpeed()
    {
      float speed = _speedModifiers.Aggregate(_ladybugConfig.Speed, (s, mod) => mod.Apply(s));
      _speed.Value = speed;
    }
  }
}