using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Utils;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Scissors
{
  public class ScissorsOpenedFacade: IFacade, IBindable, IGameStep
  {
    private readonly ScissorsConfig _config;
    private readonly EcsEntities _ladybugs;
    private readonly float _sqrActivationDistance;

    private ScissorsVisual _visual;
    private bool _isActivated;

    public ScissorsOpenedFacade(ScissorsConfig config, IEcsUniverse universe)
    {
      _config = config;
      _sqrActivationDistance = config.ActivationDistance * config.ActivationDistance;

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<FacadeRef>()
        .Collect();
    }

    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<ScissorsVisual>();
    }

    public void Step()
    {      
      if (_isActivated)
        return;
      
      foreach(EcsEntityWrapper ladybug in _ladybugs)
      {
        Transform ladybugTransform = ladybug.Get<TransformRef>().Transform;
        if (UnityUtils.SqrDistance(_visual.transform.position, ladybugTransform.position) <= _sqrActivationDistance)
        {
          _visual.PlayOpenAnimation();
          _isActivated = true;
        }
      }
    }
  }
}