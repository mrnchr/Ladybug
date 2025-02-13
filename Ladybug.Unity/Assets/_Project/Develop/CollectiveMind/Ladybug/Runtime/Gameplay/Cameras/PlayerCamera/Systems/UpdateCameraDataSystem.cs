using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera
{
  public class UpdateCameraDataSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly EcsEntities _convertedCameras;

    public UpdateCameraDataSystem(IEcsUniverse universe)
    {
      _universe = universe;

      _convertedCameras = _universe
        .FilterGame<CameraTag>()
        .Inc<ConverterRef>()
        .Collect();
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper camera in _convertedCameras)
      {
        var facade = camera.GetFacade<CameraFacade>();
        facade.CalculateCameraData();
      }
    }
  }
}