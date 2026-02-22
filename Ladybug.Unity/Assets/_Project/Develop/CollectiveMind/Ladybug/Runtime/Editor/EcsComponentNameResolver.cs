#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace CollectiveMind.Ladybug.Runtime
{
  public static class EcsComponentNameResolver
  {
    private static Dictionary<Type, string> _names = new Dictionary<Type, string>();

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
      _names = new Dictionary<Type, string>();
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
  }
}
#endif