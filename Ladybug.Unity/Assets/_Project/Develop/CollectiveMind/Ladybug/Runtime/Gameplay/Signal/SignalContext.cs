using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct SignalContext : IEcsComponent, IEcsAutoReset<SignalContext>
  {
    public SignalData Data;
    public EcsPackedEntityWithWorld TrackedEntity;
    
    public void AutoReset(ref SignalContext c)
    {
      c.Data = null;
    }
  }
}