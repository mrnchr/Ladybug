using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Timing
{
  public class TimeService : ITickable 
  {
    private readonly List<Timer> _timers = new List<Timer>();

    public void RegisterTimer(Timer timer)
    {
      _timers.Add(timer);
    }

    public void CancelTimer(Timer timer)
    {
      _timers.Remove(timer);
    }

    public void Tick()
    {
      for (int i = _timers.Count - 1; i >= 0; i--)
      {
        Timer timer = _timers[i];
        if (timer.EndTime <= Time.time)
        {
          timer.Expired = true;
          _timers.RemoveAt(i);
        }
      }
    }
  }
}