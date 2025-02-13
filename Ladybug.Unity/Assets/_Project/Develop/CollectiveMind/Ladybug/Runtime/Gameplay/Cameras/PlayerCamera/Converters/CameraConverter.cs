using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera
{
  public class CameraConverter : MonoBehaviour, IEcsConverter
  {
    private Camera _camera;

    private void Awake()
    {
      _camera = GetComponent<Camera>();
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref CameraRef cameraRef) => cameraRef.Camera = _camera);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<CameraRef>();
    }
  }
}