using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Obstacle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class DamageLadybugSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly ICollisionService _collisionSvc;
    private readonly EcsEntities _collisions;

    public DamageLadybugSystem(IEcsUniverse universe, ICollisionService collisionSvc)
    {
      _universe = universe;
      _collisionSvc = collisionSvc;

      _collisions = _universe
        .FilterMessage<TwoSideCollision>()
        .Collect();
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper col in _collisions)
      {
        ref TwoSideCollision collision = ref col.Get<TwoSideCollision>();
        CollisionInfo info = _collisionSvc.Info;
        _collisionSvc.AssignCollision(collision);
        if (_collisionSvc.TryUnpackBothEntities(_universe.Game)
          && _collisionSvc.TrySelectByComponents<ObstacleTag, LadybugTag>())
        {
          info.Target
            .Change((ref CurrentHealth health) => --health.HP)
            .Add<DamagedEvent>();
        }
      }
    }
  }
}