using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Timing
{
  public class TimerFactory 
  {
    private readonly TimeService _timeService;

    public TimerFactory(TimeService timeService)
    {
      _timeService = timeService;
    }

    public Timer CreateTimer(float duration)
    {
      var instance = new Timer
      {
        StartTime = Time.time,
        Duration = duration
      };
      
      instance.EndTime = instance.StartTime + instance.Duration;
      
      _timeService.RegisterTimer(instance);
      return instance;
    }
  }
}