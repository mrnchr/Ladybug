using CollectiveMind.Ladybug.Runtime.Infrastructure.Timing;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class ProjectInitializer : IInitializable
  {
    private readonly TickableManager _tickableManager;
    private readonly TimeService _timeService;

    public ProjectInitializer(TickableManager tickableManager, TimeService timeService)
    {
      _tickableManager = tickableManager;
      _timeService = timeService;
    }

    public void Initialize()
    {
      _tickableManager.Add(_timeService);
    }
  }
}