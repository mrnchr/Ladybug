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

    public void ConvertTo(EcsEntity entity)
    {
      EcsWorld world = entity.World;
      foreach (EcsComponentValue componentValue in Components)
      {
        IEcsPool pool = world.GetPoolEnsure(componentValue.Type);
        pool.AddRaw(entity.Entity, componentValue.Value);
      }
    }

    public void ConvertBack(EcsEntity entity)
    {
    }
  }
}