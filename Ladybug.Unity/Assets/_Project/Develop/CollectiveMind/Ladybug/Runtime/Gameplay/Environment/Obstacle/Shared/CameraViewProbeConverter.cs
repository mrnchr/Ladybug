using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class CameraViewProbeConverter : MonoBehaviour, IEcsConverter
  {
    public Transform CameraViewProbe;

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref CameraViewProbeRef cameraViewPointRef) => cameraViewPointRef.CameraViewProbe = CameraViewProbe);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<CameraViewProbeRef>();
    }
  }
}