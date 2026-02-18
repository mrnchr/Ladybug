using System;
using System.Diagnostics;

namespace CollectiveMind.Ladybug.Runtime.TriInspector
{
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
  [Conditional("UNITY_EDITOR")]
  public class OnInspectorUpdate : Attribute
  {
    public string Method { get; }

    public OnInspectorUpdate() : this("")
    {
    }

    public OnInspectorUpdate(string method)
    {
      Method = method;
    }
  }
}