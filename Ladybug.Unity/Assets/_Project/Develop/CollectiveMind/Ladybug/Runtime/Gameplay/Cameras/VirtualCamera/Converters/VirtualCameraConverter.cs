using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Unity.Cinemachine;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera
{
  public class VirtualCameraConverter : MonoBehaviour, IEcsConverter
  {
    private CinemachineCamera _camera;
    
    private void Awake()
    {
      _camera = GetComponent<CinemachineCamera>();
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