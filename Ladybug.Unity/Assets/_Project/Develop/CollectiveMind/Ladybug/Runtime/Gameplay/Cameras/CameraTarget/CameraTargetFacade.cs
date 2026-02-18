using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Utils;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class CameraTargetFacade : IFacade, IBindable, IGameFixedStep
  {
    public ReadOnlyReactiveProperty<float> Speed => _speed;

    private readonly CameraConfig _config;
    private readonly EcsEntities _ladybugs;
    private readonly ReactiveProperty<float> _speed = new ReactiveProperty<float>();
    
    private CameraTargetVisual _visual;
    private EcsEntityWrapper _entity => _visual.Entity;

    public CameraTargetFacade(CameraConfig config, IEcsUniverse universe)
    {
      _config = config;

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<FacadeRef>()
        .Collect();
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<CameraTargetVisual>();
      _entity.Replace((ref StartPosition startPosition) => startPosition.Position = _visual.transform.position);
    }

    public void FixedStep()
    {
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      {
        Transform ladybugTransform = ladybug.Get<TransformRef>().Transform;
        var ladybugFacade = ladybug.GetFacade<LadybugFacade>();
        float velocityZ = ladybugFacade.Velocity.z;
        float scrollSpeed = ladybugFacade.GetScrollSpeed() * _config.CameraSpeedMultiplier;

        if (velocityZ > scrollSpeed && ladybugTransform.position.z >= _visual.transform.position.z)
        {
          _speed.Value = 0;
          _visual.transform.SetPosition(Axis.Z, ladybugTransform.position.z);
        }
        else
        {
          _speed.Value = scrollSpeed;
        }
        
        _visual.transform.SetPosition(Axis.X, ladybugTransform.position.x);
      }
    }
  }
}