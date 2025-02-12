using CollectiveMind.Ladybug.Runtime.Gameplay.Canvas;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation;
using CollectiveMind.Ladybug.Runtime.Gameplay.PlayerCamera;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class UpdateFeature : EcsFeature
  {
    public UpdateFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CreationFeature>());
      Add(systems.Create<CameraFeature>());
      Add(systems.Create<CanvasFeature>());
    }    
  }
}