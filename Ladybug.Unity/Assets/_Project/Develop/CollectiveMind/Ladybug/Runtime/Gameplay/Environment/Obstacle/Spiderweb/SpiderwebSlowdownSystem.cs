using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Spiderweb
{
  public class SpiderwebSlowdownSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly ICollisionFilter _collisionFilter;
    private readonly EcsEntities _collisions;

    public SpiderwebSlowdownSystem(IEcsUniverse universe, ICollisionFilter collisionFilter)
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
        _collisionFilter.AssignCollision(collision);
        CollisionInfo info = _collisionFilter.Info;
        
        if (_collisionFilter.TryUnpackBothEntities(_universe.Game)
            && _collisionFilter.TrySelectByComponents<SpiderwebTag, LadybugTag>())
        {
          var ladybugFacade = info.Target.GetFacade<LadybugFacade>();

          switch (collision.Type)
          {
            case CollisionType.Enter:
              ladybugFacade.EnterSpiderweb(info.Master.PackedEntity);
              break;
            case CollisionType.Exit:
              ladybugFacade.LeaveSpiderweb(info.Master.PackedEntity);
              break;
          }
        }
      }
    }
  }
}