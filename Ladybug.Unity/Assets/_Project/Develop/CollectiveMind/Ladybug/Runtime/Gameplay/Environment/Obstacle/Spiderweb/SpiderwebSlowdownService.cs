using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Spiderweb
{
  public class SpiderwebSlowdownService : IGameStep
  {
    private readonly IEcsUniverse _universe;
    private readonly SessionService _session;
    private readonly SpiderwebConfig _config;

    private List<EcsPackedEntity> _overlapSourceIds;
    private IDisposable _speedModifier;

    public SpiderwebSlowdownService(IEcsUniverse universe,
      SessionService sessionService,
      SpiderwebConfig config)
    {
      _universe = universe;
      _session = sessionService;
      _config = config;

      _overlapSourceIds = new List<EcsPackedEntity>();
    }

    public void AddSpeedModifier(EcsPackedEntity packedEntity)
    {
      if (_overlapSourceIds.Contains(packedEntity))
        return;

      _overlapSourceIds.Add(packedEntity);

      if (_speedModifier != null)
        return;

      _speedModifier = _session.AddSpeedModifier(new SpeedModifier(SpeedModifierType.Multiply, _config.SpeedMultiplier));
    }
    
    public void DisposeSpeedModifier(EcsPackedEntity packedEntity)
    {
      if (!_overlapSourceIds.Remove(packedEntity))
        return;
      if (_overlapSourceIds.Count > 0)
        return;

      _speedModifier?.Dispose();
      _speedModifier = null;
    }

    public void Step()
    {
      RemoveDisposedEntities();
    }

    private void RemoveDisposedEntities()
    {
      for (int i = _overlapSourceIds.Count - 1; i >= 0; i--)
      {
        if (!_overlapSourceIds[i].TryUnpackEntity(_universe.Game, out EcsEntityWrapper _))
        {
          _overlapSourceIds.RemoveAt(i);
        }
      }
    }
  }
}