using System;
using System.Diagnostics;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
  [Conditional("UNITY_EDITOR")]
  public class ExistInWorldAttribute : Attribute
  {
    public string Name { get; }

    public ExistInWorldAttribute(string name)
    {
      Name = name;
    }
  }
}