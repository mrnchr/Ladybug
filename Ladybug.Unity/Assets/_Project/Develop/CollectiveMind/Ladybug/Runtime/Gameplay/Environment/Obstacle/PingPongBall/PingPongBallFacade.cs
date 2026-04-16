using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.PingPongBall
{
  public class PingPongBallFacade : IFacade, IBindable, IEntityInitializable, IGameFixedStep, IDisposable
  {
    private readonly PingPongBallConfig _config;
    private readonly IEcsUniverse _ecsUniverse;
    private readonly GameSessionData _gameSessionData;
    private readonly LadybugService _ladybugService;
    private readonly ICollisionFilter _collisionFilter;
    private EntityVisual _visual;
    private DisposableBag _disposableBag;
    
    private EcsEntityWrapper _entity => _visual.Entity;
    private bool _isMoving;
    private readonly EcsEntities _collisions;

    public PingPongBallFacade(PingPongBallConfig config,
      IEcsUniverse ecsUniverse,
      GameSessionData gameSessionData,
      LadybugService ladybugService,
      ICollisionFilter collisionFilter)
    {
      _config = config;
      _ecsUniverse = ecsUniverse;
      _gameSessionData = gameSessionData;
      _ladybugService = ladybugService;
      _collisionFilter = collisionFilter;
      
      _collisions = _ecsUniverse
        .FilterMessage<TwoSideCollision>()
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

          if (_collisionFilter.TryUnpackBothEntities(_ecsUniverse.Game)
            && _collisionFilter.TrySelectByComponents<ObstacleTag, LadybugTag>())
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

      EcsEntityWrapper ladybug = _ladybugService.Ladybug;
      
      if (!ladybug.IsAlive())
      {
        return;
      }

      Transform ladybugTransform = ladybug.Get<TransformRef>().Transform;
      Rigidbody rigidbody = _entity.Get<RigidbodyRef>().Rigidbody;

      Vector3 targetVector = ladybugTransform.position - rigidbody.position;
      Vector3 moveDirection = targetVector;
      moveDirection.y = 0;
      moveDirection.Normalize();

      float speed = _config.Speed * _gameSessionData.SpeedRate.Value;
      
      Vector3 groundPointPosition = _entity.Get<GroundPointRef>().GroundPoint.transform.position;
      float radius = Vector3.Distance(rigidbody.position, groundPointPosition);
      float angularSpeed;
      Vector3 axis;

      if (_config.UseAcceleration)
      {
        rigidbody.AddForce(moveDirection * speed, ForceMode.Acceleration);

        Vector3 velocity = rigidbody.linearVelocity;
        Vector3 planar = velocity;
        planar.y = 0;
        planar = Vector3.ClampMagnitude(planar, speed);
        rigidbody.linearVelocity = new Vector3(planar.x, velocity.y, planar.z);

        angularSpeed = planar.magnitude / radius;
        axis = Vector3.Cross(Vector3.up, planar.normalized);
      }
      else
      {
        rigidbody.linearVelocity = moveDirection * speed;
        
        angularSpeed = speed / radius;
        axis = Vector3.Cross(Vector3.up, moveDirection.normalized);
      }
        
      rigidbody.angularVelocity = axis * angularSpeed;
    }
  }
}