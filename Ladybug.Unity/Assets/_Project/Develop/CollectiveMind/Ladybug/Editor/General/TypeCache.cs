using System;
using System.Reflection;
using CollectiveMind.Ladybug.Runtime;

namespace CollectiveMind.Ladybug.Editor.General
{
  public class TypeCache : ITypeCache
  {
    private readonly MemberCache<FieldInfo> _fieldCache = new MemberCache<FieldInfo>();
    private readonly MemberCache<PropertyInfo> _propertyCache = new MemberCache<PropertyInfo>();
    private readonly MemberCache<MethodInfo> _methodCache = new MemberCache<MethodInfo>();

    public FieldInfo GetCachedField(Type type, string name, bool isPrivate = false, bool isStatic = false)
    {
      return _fieldCache.GetMember(type, name, isPrivate, isStatic);
    }

    public PropertyInfo GetCachedProperty(Type type, string name, bool isPrivate = false, bool isStatic = false)
    {
      return _propertyCache.GetMember(type, name, isPrivate, isStatic);
    }

    public MethodInfo GetCachedMethod(Type type, string name, bool isPrivate = false, bool isStatic = false)
    {
      return _methodCache.GetMember(type, name, isPrivate, isStatic);
    }
  }
}