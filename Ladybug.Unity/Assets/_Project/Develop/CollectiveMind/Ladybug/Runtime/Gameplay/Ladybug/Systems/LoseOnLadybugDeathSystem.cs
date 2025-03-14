using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using CollectiveMind.Ladybug.Runtime.UI.Defeat;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LoseOnLadybugDeathSystem : IEcsRunSystem
  {
    private readonly IWindowManager _windowManager;
    private readonly EcsEntities _damagedLadybug;

    public LoseOnLadybugDeathSystem(IEcsUniverse  universe, IWindowManager windowManager)
    {
      _windowManager = windowManager;
      _damagedLadybug = universe
        .FilterGame<LadybugTag>()
        .Inc<DamagedEvent>()
        .Collect();
    }
    
    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper evt in _damagedLadybug)
      {
        if (evt.Get<CurrentHealth>().HP <= 0)
          _windowManager.OpenWindow<DefeatWindow>();
      }
    }
  }
}