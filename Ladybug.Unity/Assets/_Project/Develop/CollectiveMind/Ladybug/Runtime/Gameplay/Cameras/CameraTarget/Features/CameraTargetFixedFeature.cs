using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  public class CameraTargetFixedFeature : EcsFeature
  {
    public CameraTargetFixedFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CalculateCameraSpeedRateSystem>());
      Add(systems.Create<ChangeCameraTargetHorizontalPositionSystem>());
      Add(systems.Create<ChangeCameraTargetVelocitySystem>());
    }
  }
}