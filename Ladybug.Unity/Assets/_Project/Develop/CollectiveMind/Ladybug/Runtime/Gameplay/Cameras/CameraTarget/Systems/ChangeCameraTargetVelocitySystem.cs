using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class ChangeCameraTargetVelocitySystem : IEcsRunSystem
  {
    private readonly GameSessionData _sessionData;
    private readonly EcsEntities _targets;
    private readonly CameraConfig _config;

    public ChangeCameraTargetVelocitySystem(IEcsUniverse universe, GameSessionData sessionData, IConfigProvider configProvider)
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
        Rigidbody rb = target.Get<RigidbodyRef>().Rigidbody;
        rb.velocity = Vector3.forward * _config.Speed * _sessionData.SpeedRate.Value
          * target.AddOrGet<CameraSpeedRate>().Rate;
      }
    }
  }
}