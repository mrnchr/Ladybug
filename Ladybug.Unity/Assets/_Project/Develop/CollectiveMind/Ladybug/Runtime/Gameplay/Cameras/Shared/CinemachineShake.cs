using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  [DisallowMultipleComponent]
  public class CinemachineShake : CinemachineExtension
  {
    private CameraShakeController _cameraShakeController;

    [Inject]
    public void Construct(CameraShakeController cameraShakeController)
    {
      _cameraShakeController = cameraShakeController;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
      CinemachineCore.Stage stage,
      ref CameraState state,
      float deltaTime)
    {
#if UNITY_EDITOR
      if (!UnityEditor.EditorApplication.isPlaying)
      {
        return;
      }
#endif
      
      if (stage != CinemachineCore.Stage.Noise)
      {
        return;
      }

      Vector2 offset = _cameraShakeController.ShakingOffset.CurrentValue;
      state.RawPosition.x += offset.x;
      state.RawPosition.z += offset.y;
    }
  }
}