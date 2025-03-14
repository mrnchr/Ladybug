using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class DamageLadybugSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly ICollisionFilter _collisionFilter;
    private readonly GameSessionData _sessionData;
    private readonly EcsEntities _collisions;

    public DamageLadybugSystem(IEcsUniverse universe, ICollisionFilter collisionFilter, GameSessionData sessionData)
    {
      _universe = universe;
      _collisionFilter = collisionFilter;
      _sessionData = sessionData;

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
          info.Target.Add<DamagedEvent>();
          _sessionData.Health.Value = Mathf.Max(0, _sessionData.Health.Value - 1);
        }
      }
    }
  }
}