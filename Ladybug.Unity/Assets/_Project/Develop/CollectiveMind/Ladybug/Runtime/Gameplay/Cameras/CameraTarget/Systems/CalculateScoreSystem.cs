using CollectiveMind.Ladybug.Runtime.Gameplay.Creation;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class CalculateScoreSystem : IEcsRunSystem
  {
    private readonly GameSessionData _sessionData;
    private readonly EcsEntities _targets;
    private readonly EcsEntities _convertedTargets;

    public CalculateScoreSystem(IEcsUniverse universe, GameSessionData sessionData)
    {
      _sessionData = sessionData;

      _convertedTargets = universe
        .FilterGame<CameraTargetTag>()
        .Inc<OnConverted>()
        .Collect();

      _targets = universe
        .FilterGame<CameraTargetTag>()
        .Inc<ConverterRef>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper target in _convertedTargets)
      {
        target.Replace((ref StartPosition startPosition) =>
          startPosition.Position = target.Get<TransformRef>().Transform.position);
      }

      foreach (EcsEntityWrapper target in _targets)
      {
        Vector3 position = target.Get<TransformRef>().Transform.position;
        _sessionData.Score.Value = position.z - target.Get<StartPosition>().Position.z;
      }
    }
  }
}