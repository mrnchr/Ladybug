using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class ChangeCameraTargetPositionSystem : IEcsRunSystem
  {
    private readonly EcsEntities _targets;
    private readonly EcsEntities _ladybugs;

    public ChangeCameraTargetPositionSystem(IEcsUniverse universe)
    {
      _targets = universe
        .FilterGame<CameraTargetTag>()
        .Inc<ConverterRef>()
        .Collect();

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<ConverterRef>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      foreach (EcsEntityWrapper target in _targets)
      {
        Vector3 ladybugPosition = ladybug.Get<TransformRef>().Transform.position;
        Transform targetTransform = target.Get<TransformRef>().Transform;

        Vector3 position = ladybugPosition;
        position.y = targetTransform.position.y;
        targetTransform.position = position;
      }
    }
  }
}