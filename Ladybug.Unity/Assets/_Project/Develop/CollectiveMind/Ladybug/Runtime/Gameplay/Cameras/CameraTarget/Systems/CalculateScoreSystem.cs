using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class CalculateScoreSystem : IEcsRunSystem
  {
    private readonly SessionService _session;
    private readonly EcsEntities _targets;

    public CalculateScoreSystem(IEcsUniverse universe, SessionService session)
    {
      _session = session;

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
        _session.Score.Value = position.z - target.Get<StartPosition>().Position.z;
      }
    }
  }
}