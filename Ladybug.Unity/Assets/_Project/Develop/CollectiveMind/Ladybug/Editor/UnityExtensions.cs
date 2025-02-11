using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using TypeCache = CollectiveMind.Ladybug.Editor.General.TypeCache;

namespace CollectiveMind.Ladybug.Editor
{
  public static class UnityExtensions
  {
    private static readonly TypeCache _typeCache = new TypeCache();
    private static readonly Type _type = typeof(ScriptableSingleton<>);

    public static void Save<T>(this ScriptableSingleton<T> singleton, bool saveAsText = true) where T : ScriptableObject
    {
      Type genericType = _type.MakeGenericType(typeof(T));
      MethodInfo saveMethod = _typeCache.GetCachedMethod(genericType, "Save", true);
      saveMethod.Invoke(singleton, new object[] { saveAsText });
      EditorUtility.ClearDirty(singleton);
    }
  }
}