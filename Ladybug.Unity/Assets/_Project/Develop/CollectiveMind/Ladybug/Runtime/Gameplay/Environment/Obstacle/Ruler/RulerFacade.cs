using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Ruler
{
  public class RulerFacade : IFacade, IBindable, IEntityInitializable, IDisposable
  {
    private readonly IEcsUniverse _ecsUniverse;
    private readonly RulerConfig _config;
    private DisposableBag _disposableBag;

    private EntityVisual _visual;
    private EcsEntityWrapper _entity => _visual.Entity;

    public RulerFacade(IEcsUniverse ecsUniverse, RulerConfig config)
    {
      _ecsUniverse = ecsUniverse;
      _config = config;
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<EntityVisual>();
      _disposableBag.Add(_ecsUniverse.Subscribe<InCameraView>(_entity, OnCameraViewEnter));
    }

    public void Initialize(EntityInitContext initContext)
    {
      _entity.Get<RigidbodyRef>().Rigidbody.useGravity = false;
      _visual.transform.Rotate(-_config.RulerSpawnAngle, 0f, 0f, Space.Self);
    }

    private void OnCameraViewEnter()
    {
      _entity.Get<RigidbodyRef>().Rigidbody.useGravity = true;
    }

    public void Dispose()
    {
      _disposableBag.Dispose();
    }
  }
}