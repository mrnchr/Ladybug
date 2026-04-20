using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class DetectInCameraViewSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _ecsUniverse;
    private readonly ICollisionFilter _collisionFilter;
    private readonly EcsEntities _collisions;

    public DetectInCameraViewSystem(IEcsUniverse ecsUniverse, ICollisionFilter collisionFilter)
    {
      _ecsUniverse = ecsUniverse;
      _collisionFilter = collisionFilter;

      _collisions = ecsUniverse
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
        
        if (_collisionFilter.TryUnpackBothEntities(_ecsUniverse.Game)
          && _collisionFilter.TrySelectByComponents<CameraTag, TrackedInCameraView>())
        {
          bool inCameraView = info.Target.Has<InCameraView>();
          
          if (!inCameraView && info.Type == CollisionType.Enter)
          {
            info.Target.Add<InCameraView>();
            _ecsUniverse.Publish<InCameraView>(info.Target);
          }
          else if (inCameraView && info.Type == CollisionType.Exit)
          {
            info.Target.Del<InCameraView>();
            _ecsUniverse.Publish<OutCameraView>(info.Target);
          }
        }
      }
    }
  }
}