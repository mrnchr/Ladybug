using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class CalculateCameraSpeedRateSystem : IEcsRunSystem
  {
    private readonly EcsEntities _targets;
    private readonly EcsEntities _ladybugs;
    private readonly EcsEntities _cameras;
    private readonly CameraConfig _config;

    public CalculateCameraSpeedRateSystem(IEcsUniverse universe, IConfigProvider configProvider)
    {
      _config = configProvider.Get<CameraConfig>();
      
      _targets = universe
        .FilterGame<CameraTargetTag>()
        .Inc<ConverterRef>()
        .Collect();

      _cameras = universe
        .FilterGame<CameraTag>()
        .Collect();

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<ConverterRef>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      foreach (EcsEntityWrapper camera in _cameras)
      foreach (EcsEntityWrapper target in _targets)
      {
        Rect bounds = camera.Get<CameraData>().WorldDeepBounds;
        Vector3 ladybugPosition = ladybug.Get<TransformRef>().Transform.position;
        
        float ratio = Mathf.Clamp01((ladybugPosition.z - bounds.yMin) / (bounds.yMax - bounds.yMin));
        target.Replace((ref CameraSpeedRate rate) =>
          rate.Rate = _config.SpeedRates[Mathf.FloorToInt(ratio * _config.SpeedRates.Count)]);
      }
    }
  }
}