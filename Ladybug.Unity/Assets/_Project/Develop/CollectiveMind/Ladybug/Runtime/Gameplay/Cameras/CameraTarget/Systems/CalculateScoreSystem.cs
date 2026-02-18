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

    public CalculateScoreSystem(IEcsUniverse universe, GameSessionData sessionData)
    {
      _sessionData = sessionData;

      _targets = universe
        .FilterGame<CameraTargetTag>()
        .Inc<GameObjectRef>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper target in _targets)
      {
        Vector3 position = target.Get<TransformRef>().Transform.position;
        _sessionData.Score.Value = position.z - target.Get<StartPosition>().Position.z;
      }
    }
  }
}