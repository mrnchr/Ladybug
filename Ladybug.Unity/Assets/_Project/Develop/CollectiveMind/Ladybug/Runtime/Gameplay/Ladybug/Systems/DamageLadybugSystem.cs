using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class DamageLadybugSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly ICollisionFilter _collisionFilter;
    private readonly SessionService _session;
    private readonly EcsEntities _collisions;

    public DamageLadybugSystem(IEcsUniverse universe, ICollisionFilter collisionFilter, SessionService session)
    {
      _universe = universe;
      _collisionFilter = collisionFilter;
      _session = session;

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
          && _collisionFilter.TrySelectByComponents<DamageSource, LadybugTag>()
          && !info.Target.Has<Invincible>())
        {
          _session.SubtractHealth(1);
          _universe.Publish<OnDamageEvent>(info.Master);
        }
      }
    }
  }
}