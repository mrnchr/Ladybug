using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class CameraTargetVisual : EntityVisual, IBindable
  {
    private CameraTargetFacade _facade;
    private Rigidbody _rigidbody;

    [Inject]
    public void Construct()
    {
      _rigidbody = GetComponent<Rigidbody>();
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _facade = entity.GetFacade<CameraTargetFacade>();
      _facade.Speed.Subscribe(ApplyVelocity).AddTo(this);
    }

    private void ApplyVelocity(float speed)
    {
      _rigidbody.linearVelocity = transform.forward * speed;
    }
  }
}