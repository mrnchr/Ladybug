using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Unity.Cinemachine;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public class LadybugCameraObserver : IDisposable
  {
    private readonly CameraConfig _cameraConfig;
    private readonly GameSessionData _sessionData;
    private readonly EcsEntities _ladybugs;
    private readonly EcsEntities _cameras;

    public LadybugCameraObserver(IEcsUniverse universe, CameraConfig cameraConfig, GameSessionData sessionData)
    {
      _cameraConfig = cameraConfig;
      _sessionData = sessionData;

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<VisualFacadeRef>()
        .Exc<Boosting>()
        .Collect();

      _cameras = universe
        .FilterGame<CameraTag>()
        .Inc<CameraData>()
        .Collect();

      CinemachineCore.CameraUpdatedEvent.AddListener(CheckLadybugPosition);
    }

    public void Dispose()
    {
      CinemachineCore.CameraUpdatedEvent.RemoveListener(CheckLadybugPosition);
    }

    private void CheckLadybugPosition(CinemachineBrain _)
    {
      foreach (EcsEntityWrapper camera in _cameras)
      {
        var cameraFacade = camera.GetFacade<CameraFacade>();
        cameraFacade.CalculateCameraData();
        Rect cameraBounds = camera.Get<CameraData>().WorldXZBounds;
        
        foreach (EcsEntityWrapper ladybug in _ladybugs)
        {
          Transform transform = ladybug.Get<TransformRef>().Transform;

          if (transform.position.z < cameraBounds.yMin - _cameraConfig.FrameOffset)
          {
            var ladybugFacade = ladybug.GetFacade<LadybugFacade>();
            _sessionData.Health.Value = Mathf.Max(0, _sessionData.Health.Value - 1);
            
            ladybugFacade.Boost();
          }
        }
      }
    }
  }
}