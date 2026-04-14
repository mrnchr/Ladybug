using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugService
  {
    public EcsEntityWrapper Ladybug
    {
      get
      {
        if (!_ladybug.IsAlive())
        {
          foreach (EcsEntityWrapper ladybug in _ladybugs)
          {
            _ladybug.Copy(ladybug);
            break;
          }
        }
        
        return _ladybug;
      }
    }

    private readonly EcsEntityWrapper _ladybug = new EcsEntityWrapper();
    private readonly EcsEntities _ladybugs;

    public LadybugService(IEcsUniverse ecsUniverse)
    {
      _ladybugs = ecsUniverse
        .FilterGame<LadybugTag>()
        .Collect();
    }
  }
}