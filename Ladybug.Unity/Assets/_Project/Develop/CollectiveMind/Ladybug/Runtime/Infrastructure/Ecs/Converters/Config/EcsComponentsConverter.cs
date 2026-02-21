using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Utils;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  public class EcsComponentsConverter : IEcsConverter
  {
    public List<EcsComponentValue> Components;

    public void ConvertTo(EcsEntityWrapper entity)
    {
      EcsWorld world = entity.World;

      foreach (EcsComponentValue componentValue in Components)
      {
        IEcsPool pool = world.GetPoolEnsure(componentValue.Type);

        if (pool.Has(entity.Entity))
        {
          pool.SetRaw(entity.Entity, componentValue.Value);
        }
        else
        {
          pool.AddRaw(entity.Entity, componentValue.Value);
        }
      }
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      EcsWorld world = entity.World;

      foreach (EcsComponentValue componentValue in Components)
      {
        IEcsPool pool = world.GetPoolEnsure(componentValue.Type);

        if (pool.Has(entity.Entity))
        {
          pool.Del(entity.Entity);
        }
      }
    }
    
#if UNITY_EDITOR
    public IReadOnlyList<Type> ComponentTypes
    {
      get
      {
        _componentTypes.Clear();

        foreach (EcsComponentValue componentValue in Components)
        {
          _componentTypes.Add(componentValue.Type);
        }

        return _componentTypes;
      }
    }

    private List<Type> _componentTypes = new List<Type>();
#endif
  }
}