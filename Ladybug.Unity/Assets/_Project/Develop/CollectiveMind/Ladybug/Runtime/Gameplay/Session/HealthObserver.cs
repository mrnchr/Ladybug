using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Cysharp.Threading.Tasks;
using R3;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Session
{
  public class HealthObserver : IDisposable
  {
    private readonly GameSessionData _sessionData;
    private readonly GameSessionController _gameSessionController;
    private readonly CameraShakeController _cameraShakeController;
    private readonly EcsEntities _ladybugs;
    private DisposableBag _disposables;

    public HealthObserver(GameSessionData sessionData,
      GameSessionController gameSessionController,
      CameraShakeController cameraShakeController,
      IEcsUniverse universe)
    {
      _sessionData = sessionData;
      _gameSessionController = gameSessionController;
      _cameraShakeController = cameraShakeController;
      
      _disposables.Add(_sessionData.Health
        .Pairwise()
        .Where(pair => pair.Current < pair.Previous)
        .Select(pair => pair.Current)
        .Subscribe(OnHealthChanged));

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<FacadeRef>()
        .Collect();
    }

    public void Dispose()
    {
      _disposables.Dispose();
    }

    private void OnHealthChanged(int health)
    {
      if (health <= 0)
      {
        _gameSessionController.Defeat().Forget();
        return;
      }

      _cameraShakeController.Shake();

      foreach (EcsEntityWrapper ladybug in _ladybugs)
      {
        var facade = ladybug.GetFacade<LadybugFacade>();
        facade.TurnOnInvincibility();
      }
    }
  }
}