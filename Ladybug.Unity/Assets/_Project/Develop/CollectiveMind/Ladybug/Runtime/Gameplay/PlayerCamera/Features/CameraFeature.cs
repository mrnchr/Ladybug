using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.PlayerCamera
{
  public class CameraFeature : EcsFeature
  {
    public CameraFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<UpdateCameraDataSystem>());
    }
  }
}