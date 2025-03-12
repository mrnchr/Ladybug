using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  [Serializable]
  public class HealthConverter : ISerializedEcsConverter
  {
    public int DefaultHealth;

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity
        .Replace((ref DefaultHealth health) => health.HP = DefaultHealth)
        .Replace((ref CurrentHealth health) => health.HP = DefaultHealth);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity
        .Del<CurrentHealth>()
        .Del<DefaultHealth>();
    }
  }
}