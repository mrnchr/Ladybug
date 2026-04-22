using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  public struct SignalInitContext
  {
    public SignalType Type;
    public EcsPackedEntityWithWorld TrackedEntity;
  }
}