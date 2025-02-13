using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera
{
  public class CameraVisual : MonoBehaviour, IEcsConverter
  {
    private CameraFacade _facade;

    [Inject]
    public void Construct(IInstantiator instantiator)
    {
      var converter = GetComponent<GameObjectConverter>();
      _facade = instantiator.Instantiate<CameraFacade>();
      
      _facade.SetVisual(converter.EntityWrapper);
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref VisualFacadeRef facadeRef) => facadeRef.Facade = _facade);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<VisualFacadeRef>();
    }
  }
}