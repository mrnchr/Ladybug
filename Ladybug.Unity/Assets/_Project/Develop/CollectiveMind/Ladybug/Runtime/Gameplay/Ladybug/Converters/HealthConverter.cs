using System;
using System.Collections.Generic;
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
        .Has<CurrentHealth>(false)
        .Has<DefaultHealth>(false);
    }

#if UNITY_EDITOR
    private static readonly List<Type> _componentTypes = new List<Type>
    {
      typeof(DefaultHealth),
      typeof(CurrentHealth)
    };

    public IReadOnlyList<Type> ComponentTypes => _componentTypes;
#endif
  }
}