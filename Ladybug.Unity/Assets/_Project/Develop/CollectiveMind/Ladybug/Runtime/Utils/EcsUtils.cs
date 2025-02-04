using System;
using System.Reflection;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Utils
{
  public static class EcsUtils
  {
    private static readonly MethodInfo _getPoolMethod = typeof(EcsWorld).GetMethod("GetPool");

    public static IEcsPool GetPoolEnsure(this EcsWorld world, Type type)
    {
      return world.GetPoolByType(type)
        ?? (IEcsPool)_getPoolMethod.MakeGenericMethod(type).Invoke(world, Array.Empty<object>());
    }
  }
}