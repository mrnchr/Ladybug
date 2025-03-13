using CollectiveMind.Ladybug.Runtime.Gameplay.Creation;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using R3;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.HUD
{
  public class HealthBarFacade : ITickable
  {
    private readonly IEcsUniverse _universe;
    private readonly EcsEntities _damagedEvents;
    private readonly EcsEntities _convertedLadybug;

    public ReactiveProperty<int> HP { get; } = new ReactiveProperty<int>();

    public HealthBarFacade(IEcsUniverse universe)
    {
      _universe = universe;

      _convertedLadybug = _universe
        .FilterGame<LadybugTag>()
        .Inc<OnConverted>()
        .Collect();

      _damagedEvents = _universe
        .FilterGame<DamagedEvent>()
        .Inc<LadybugTag>()
        .Collect();
    }

    public void Tick()
    {
      foreach (EcsEntityWrapper ladybug in _convertedLadybug)
      {
        HP.Value = ladybug.Get<CurrentHealth>().HP;
      }
      
      foreach (EcsEntityWrapper evt in _damagedEvents)
      {
        HP.Value = evt.Get<CurrentHealth>().HP;
      }
    }
  }
}