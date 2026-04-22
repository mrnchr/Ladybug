using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Tape
{
  public class TapeFacade : IFacade, IBindable, IEntityInitializable, IGameFixedStep, IDisposable
  {
    private readonly TapeConfig _config;
    private readonly IEcsUniverse _ecsUniverse;
    private readonly ICollisionFilter _collisionFilter;
    private readonly SessionService _session;
    private readonly EcsEntities _collisions;
    private EntityVisual _visual;
    private DisposableBag _disposableBag;
    
    private EcsEntityWrapper _entity => _visual.Entity;
    private Vector3 _moveDirection;
    private bool _isMoving;

    public TapeFacade(TapeConfig config,
      IEcsUniverse ecsUniverse,
      ICollisionFilter collisionFilter,
      SessionService session)
    {
      _config = config;
      _ecsUniverse = ecsUniverse;
      _collisionFilter = collisionFilter;
      _session = session;

      _collisions = _ecsUniverse
        .FilterMessage<CollisionMessage>()
        .Collect();
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<EntityVisual>();
      _disposableBag.Add(_ecsUniverse.Subscribe<InCameraView>(_entity, OnCameraViewEnter));
    }

    public void Initialize(EntityInitContext _)
    {
      _entity.Get<RigidbodyRef>().Rigidbody.useGravity = false;
      _isMoving = true;
      _moveDirection = _visual.transform.forward;
      _moveDirection.y = 0;
      _moveDirection.Normalize();
    }

    public void FixedStep()
    {
      if (_entity.IsAlive())
      {
        TrackCollisions();
        Move();
      }
    }

    public void Dispose()
    {
      _disposableBag.Dispose();
    }

    private void OnCameraViewEnter()
    {
      _entity.Get<RigidbodyRef>().Rigidbody.useGravity = true;
    }

    private void TrackCollisions()
    {
      if (_entity.Has<InCameraView>())
      {
        foreach (EcsEntityWrapper collision in _collisions)
        {
          _collisionFilter.AssignCollision(collision);
          CollisionInfo info = _collisionFilter.Info;

          if (_collisionFilter.TryUnpackAnyEntity(_ecsUniverse.Game)
            && _collisionFilter.TrySelectByMasterEntity(x => x.Entity == _entity.Entity)
            && (!info.Target.IsAlive() || !info.Target.Has<CanvasTag>()))
          {
            _isMoving = false;
          }
        }
      }
    }

    private void Move()
    {
      if (!_isMoving)
      {
        return;
      }
        
      Transform transform = _entity.Get<TransformRef>().Transform;
      Rigidbody rigidbody = _entity.Get<RigidbodyRef>().Rigidbody;
      float speed = _config.Speed * _session.SpeedRate.Value;
      rigidbody.linearVelocity = _moveDirection * speed;

      Vector3 groundPointPosition = _entity.Get<GroundPointRef>().GroundPoint.transform.position;
      float radius = Vector3.Distance(transform.position, groundPointPosition);
      float angularSpeed = speed / radius;
      rigidbody.angularVelocity = transform.right * angularSpeed;
    }
  }
}