using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class ChangeCameraTargetSpeedSystem : IEcsRunSystem
  {
    private readonly GameSessionData _sessionData;
    private readonly EcsEntities _targets;
    private readonly CameraConfig _config;

    public ChangeCameraTargetSpeedSystem(IEcsUniverse universe,
      GameSessionData sessionData,
      IConfigProvider configProvider)
    {
      _sessionData = sessionData;
      _config = configProvider.Get<CameraConfig>();

      _targets = universe
        .FilterGame<CameraTargetTag>()
        .Inc<RigidbodyRef>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper target in _targets)
      {
        ref MovementSpeed movementSpeed = ref target.AddOrGet<MovementSpeed>();
        float rawSpeed = _config.Speed * _sessionData.SpeedRate.Value * target.AddOrGet<CameraSpeedRate>().Rate;
        float delta = rawSpeed - movementSpeed.Speed;
        float step = delta / _config.SmoothTime * Time.fixedDeltaTime;
        if (Mathf.Abs(step) < _config.MinSmoothStep)
          step = Mathf.Sign(step) * _config.MinSmoothStep;

        if (Mathf.Abs(step) > Mathf.Abs(delta))
          step = delta;
        
        movementSpeed.Speed += step;
        Rigidbody rb = target.Get<RigidbodyRef>().Rigidbody;
        rb.velocity = Vector3.forward * movementSpeed.Speed;
      }
    }
  }
}