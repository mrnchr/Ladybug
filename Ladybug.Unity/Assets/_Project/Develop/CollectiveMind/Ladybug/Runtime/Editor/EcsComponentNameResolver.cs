#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using UnityEditor;

namespace CollectiveMind.Ladybug.Runtime
{
  public static class EcsComponentNameResolver
  {
    private static Dictionary<Type, string> _names = CreateDictionary();

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
      _names = CreateDictionary();
    }

    public static string GetComponentName(Type type)
    {
      string name;

      if (type == null)
      {
        name = "[None]";
      }
      else if (!_names.TryGetValue(type, out name))
      {
        name = ObjectNames.NicifyVariableName(type.Name);
        _names[type] = name;
      }

      return name;
    }

    private static Dictionary<Type, string> CreateDictionary()
    {
      return new Dictionary<Type, string>
      {
        { typeof(YawDeviationRange), YawDeviationRange.ANGULAR_DEVIATION }
      };
    }
  }
}
#endif