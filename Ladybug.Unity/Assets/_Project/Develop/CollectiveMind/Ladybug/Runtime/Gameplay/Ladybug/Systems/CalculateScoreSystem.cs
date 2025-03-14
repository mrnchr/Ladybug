using CollectiveMind.Ladybug.Runtime.Gameplay.Creation;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class CalculateScoreSystem : IEcsRunSystem
  {
    private readonly GameSessionData _sessionData;
    private readonly EcsEntities _ladybugs;

    public CalculateScoreSystem(IEcsUniverse universe, GameSessionData sessionData)
    {
      _sessionData = sessionData;
      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<ConverterRef>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      {
        ref LastPosition lastPosition = ref ladybug.AddOrGet<LastPosition>();
        Vector3 position = ladybug.Get<TransformRef>().Transform.position;
        if (!ladybug.Has<OnConverted>())
          _sessionData.Score.Value += Vector3.Distance(position, lastPosition.Position);

        lastPosition.Position = position;
      }
    }
  }
}