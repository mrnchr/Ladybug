using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Canvas;
using CollectiveMind.Ladybug.Runtime.Gameplay.Collisions;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class UpdateFeature : EcsFeature
  {
    public UpdateFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CollisionFeature>());
      Add(systems.Create<CreationFeature>());
      Add(systems.Create<VirtualCameraFeature>());
      Add(systems.Create<CameraFeature>());
      Add(systems.Create<LadybugFeature>());
      Add(systems.Create<CanvasFeature>());
    }    
  }
}