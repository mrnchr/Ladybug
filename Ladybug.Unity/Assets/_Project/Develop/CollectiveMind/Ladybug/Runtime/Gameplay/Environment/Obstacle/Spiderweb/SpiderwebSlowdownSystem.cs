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
    private readonly SpiderwebSlowdownService _slowdownService;
    private readonly EcsEntities _collisions;

    public SpiderwebSlowdownSystem(IEcsUniverse universe,
      ICollisionFilter collisionFilter,
      SpiderwebSlowdownService slowdownService)
    {
      _universe = universe;
      _collisionFilter = collisionFilter;
      _slowdownService = slowdownService;

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
             _slowdownService.DisposeSpeedModifier(info.Master.PackedEntity);
            continue;
          }
          
          if (info.Target.Has<Invincible>())
            continue;

          _slowdownService.AddSpeedModifier(info.Master.PackedEntity);
        }
      }
    }
  }
}