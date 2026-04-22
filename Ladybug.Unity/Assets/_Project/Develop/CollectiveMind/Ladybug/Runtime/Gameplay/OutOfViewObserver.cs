using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Unity.Cinemachine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public class OutOfViewObserver : IDisposable
  {
    private readonly SessionService _session;
    private readonly CameraService _cameraService;
    private readonly EcsEntities _ladybugs;

    public OutOfViewObserver(IEcsUniverse universe, SessionService session, CameraService cameraService)
    {
      _session = session;
      _cameraService = cameraService;

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<FacadeRef>()
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
          _session.SubtractHealth(1);

          var ladybugFacade = ladybug.GetFacade<LadybugFacade>();
          ladybugFacade.Boost();
        }
      }
    }
  }
}