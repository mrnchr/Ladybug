using Cinemachine;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera
{
  public class VirtualCameraConverter : MonoBehaviour, IEcsConverter
  {
    private CinemachineVirtualCamera _camera;
    
    private void Awake()
    {
      _camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref VirtualCameraRef virtualCameraRef) => virtualCameraRef.Camera = _camera);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<VirtualCameraRef>();
    }
  }
}