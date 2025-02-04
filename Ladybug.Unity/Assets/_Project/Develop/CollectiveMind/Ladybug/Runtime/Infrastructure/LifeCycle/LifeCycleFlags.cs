using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle
{
  [Flags]
  public enum LifeCycleFlags
  {
    None = 0,
    Initialize = 1 << 0,
    FixedTick = 1 << 1,
    Tick = 1 << 2,
    LateTick = 1 << 3,
    Dispose = 1 << 4,
    LateDispose = 1 << 5,
    All = int.MaxValue
  }
}