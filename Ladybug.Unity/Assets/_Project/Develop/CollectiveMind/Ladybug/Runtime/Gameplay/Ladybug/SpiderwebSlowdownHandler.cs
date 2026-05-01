using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Spiderweb;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Utils;
using Leopotam.EcsLite;
using R3;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class SpiderwebSlowdownHandler : IDisposable
  {
    private readonly IEcsUniverse _universe;
    private readonly SessionService _session;
    private readonly SpiderwebConfig _config;
    private readonly LadybugFacade _facade;
    private readonly List<EcsPackedEntity> _overlapSourceIds;
    private DisposableBag _disposableBag;

    private IDisposable _speedModifier;

    public SpiderwebSlowdownHandler(IEcsUniverse universe,
      SessionService sessionService,
      SpiderwebConfig config,
      LadybugFacade facade)
    {
      _universe = universe;
      _session = sessionService;
      _config = config;
      _facade = facade;

      _overlapSourceIds = new List<EcsPackedEntity>();
      _disposableBag.Add(facade.IsInvincible.Subscribe(_ => UpdateSpeedModifier()));
    }

    public void EnterSpiderweb(EcsPackedEntity packedEntity)
    {
      if (_overlapSourceIds.Contains(packedEntity))
        return;

      _overlapSourceIds.Add(packedEntity);
      UpdateSpeedModifier();
    }

    public void LeaveSpiderweb(EcsPackedEntity packedEntity)
    {
      if (!_overlapSourceIds.Remove(packedEntity))
        return;
      
      UpdateSpeedModifier();
    }

    public void Step()
    {
      RemoveDisposedEntities();
      UpdateSpeedModifier();
    }

    public void Dispose()
    {
      _disposableBag.Dispose();
      CSharpUtils.DisposeAndClear(ref _speedModifier);
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

    private void UpdateSpeedModifier()
    {
      bool hasSpeedModifier = !_facade.IsInvincible.CurrentValue && _overlapSourceIds.Count > 0;

      if (hasSpeedModifier && _speedModifier == null)
      {
        _speedModifier = _session.AddSpeedModifier(new SpeedModifier(SpeedModifierType.Multiply, _config.SpeedMultiplier));
      }
      else if (!hasSpeedModifier && _speedModifier != null)
      {
        CSharpUtils.DisposeAndClear(ref _speedModifier);
      }
    }
  }
}