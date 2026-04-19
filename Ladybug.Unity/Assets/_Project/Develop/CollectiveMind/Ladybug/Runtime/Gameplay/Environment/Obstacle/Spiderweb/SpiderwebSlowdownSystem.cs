using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Spiderweb
{
  public class SpiderwebSlowdownSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly ICollisionFilter _collisionFilter;
    private readonly SessionService _session;
    private readonly EcsEntities _collisions;
    private readonly SpiderwebConfig _config;
    private readonly Dictionary<EcsPackedEntity, IDisposable> _speedModifiers;

    public SpiderwebSlowdownSystem(IEcsUniverse universe,
      ICollisionFilter collisionFilter,
      SessionService session,
      SpiderwebConfig config)
    {
      _config = config;
      _universe = universe;
      _collisionFilter = collisionFilter;
      _session = session;
      _speedModifiers = new Dictionary<EcsPackedEntity, IDisposable>();

      _collisions = _universe
        .FilterMessage<TwoSideCollision>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper col in _collisions)
      {
        ref TwoSideCollision collision = ref col.Get<TwoSideCollision>();
        _collisionFilter.AssignCollision(collision);
        CollisionInfo info = _collisionFilter.Info;
        
        if (_collisionFilter.TryUnpackBothEntities(_universe.Game)
            && _collisionFilter.TrySelectByComponents<SpiderwebTag, LadybugTag>())
        {
          if (collision.Type == CollisionType.Exit)
          {
            DisposeSpeedModifier(info.Master.PackedEntity);
            continue;
          }
          
          if (info.Target.Has<Invincible>())
            continue;

          AddSpeedModifier(info.Master.PackedEntity);
        }
      }
    }

    private void AddSpeedModifier(EcsPackedEntity packedEntity)
    {
      if (_speedModifiers.ContainsKey(packedEntity))
        return;

      IDisposable modifier = _session.AddSpeedModifier(new SpeedModifier(SpeedModifierType.Multiply, _config.SpeedMultiplier));
      _speedModifiers.Add(packedEntity, modifier);
    }
    
    private void DisposeSpeedModifier(EcsPackedEntity packedEntity)
    {
      if (!_speedModifiers.Remove(packedEntity, out IDisposable m))
        return;

      m.Dispose();
    }
  }
}