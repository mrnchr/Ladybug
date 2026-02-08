using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Unity.Cinemachine;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public class OutOfViewObserver : IDisposable
  {
    private readonly GameSessionData _sessionData;
    private readonly CameraService _cameraService;
    private readonly EcsEntities _ladybugs;

    public OutOfViewObserver(IEcsUniverse universe, GameSessionData sessionData, CameraService cameraService)
    {
      _sessionData = sessionData;
      _cameraService = cameraService;

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<VisualFacadeRef>()
        .Exc<Boosting>()
        .Collect();
    }

    public void Dispose()
    {
      Stop();
    }

    public void Start()
    {
      CinemachineCore.CameraUpdatedEvent.AddListener(CheckLadybugPosition);
    }

    public void Stop()
    {
      CinemachineCore.CameraUpdatedEvent.RemoveListener(CheckLadybugPosition);
    }

    private void CheckLadybugPosition(CinemachineBrain _)
    {
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      {
        if (_cameraService.IsEntityOutsideCamera(ladybug))
        {
          _sessionData.Health.Value = Mathf.Max(0, _sessionData.Health.Value - 1);

          var ladybugFacade = ladybug.GetFacade<LadybugFacade>();
          ladybugFacade.Boost();
        }
      }
    }
  }
}