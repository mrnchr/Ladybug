using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class CameraTargetVisual : MonoBehaviour, IEcsConverter
  {
    public EcsEntityWrapper Entity => _converter.EntityWrapper;
    
    private IFacadeRegistry _facadeRegistry;
    private CameraTargetFacade _facade;
    private GameObjectConverter _converter;
    private Rigidbody _rigidbody;

    [Inject]
    public void Construct(IFacadeRegistry facadeRegistry)
    {
      _facadeRegistry = facadeRegistry;
      _facade = _facadeRegistry.GetFacade<CameraTargetFacade>();
      _converter = GetComponent<GameObjectConverter>();
      _rigidbody = GetComponent<Rigidbody>();
      
      _facade.Bind(this);
      _facade.Speed.Subscribe(ApplyVelocity).AddTo(this);
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref VisualFacadeRef facadeRef) => facadeRef.Facade = _facade);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<VisualFacadeRef>();
    }

    private void OnDestroy()
    {
      _facadeRegistry.DisposeFacade(_facade);
    }

    private void ApplyVelocity(float speed)
    {
      _rigidbody.linearVelocity = transform.forward * speed;
    }
  }
}