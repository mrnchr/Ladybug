using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas
{
  public class CanvasFeature : EcsFeature
  {
    public CanvasFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<FillScreenSpaceByCanvasesSystem>());
    } 
  }
}