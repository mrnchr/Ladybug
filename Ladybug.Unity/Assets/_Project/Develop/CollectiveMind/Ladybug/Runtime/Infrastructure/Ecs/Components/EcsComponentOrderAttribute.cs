using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [AttributeUsage(AttributeTargets.Struct)]
  public class EcsComponentOrderAttribute : Attribute
  {
    public int Order { get; }

    public EcsComponentOrderAttribute(int order)
    {
      Order = order;
    }
  }
}