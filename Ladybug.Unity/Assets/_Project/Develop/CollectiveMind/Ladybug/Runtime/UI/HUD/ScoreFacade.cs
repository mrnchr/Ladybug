using CollectiveMind.Ladybug.Runtime.Gameplay.Creation;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.HUD
{
  public class ScoreFacade : ITickable
  {
    private readonly EcsEntities _ladybugs;

    private Vector3 _lastPosition;

    public ReactiveProperty<float> Score { get; } = new ReactiveProperty<float>();

    public ScoreFacade(IEcsUniverse universe)
    {
      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<ConverterRef>()
        .Collect();
    }

    public void Tick()
    {
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      {
        Vector3 position = ladybug.Get<TransformRef>().Transform.position;
        if (!ladybug.Has<OnConverted>())
          Score.Value += Vector3.Distance(position, _lastPosition);

        _lastPosition = position;
      }
    }
  }
}