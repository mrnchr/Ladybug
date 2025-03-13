using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Obstacle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class DamageLadybugSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly ICollisionFilter _collisionFilter;
    private readonly EcsEntities _collisions;

    public DamageLadybugSystem(IEcsUniverse universe, ICollisionFilter collisionFilter)
    {
      _universe = universe;
      _collisionFilter = collisionFilter;

      _collisions = _universe
        .FilterMessage<TwoSideCollision>()
        .Collect();
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper col in _collisions)
      {
        ref TwoSideCollision collision = ref col.Get<TwoSideCollision>();
        CollisionInfo info = _collisionFilter.Info;
        _collisionFilter.AssignCollision(collision);
        if (_collisionFilter.TryUnpackBothEntities(_universe.Game)
          && _collisionFilter.TrySelectByComponents<ObstacleTag, LadybugTag>())
        {
          info.Target
            .Change((ref CurrentHealth health) => --health.HP)
            .Add<DamagedEvent>();
        }
      }
    }
  }
}