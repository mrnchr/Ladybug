using System;
using System.Collections.Generic;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface ISerializedEcsConverter : IEcsConverter
  {
#if UNITY_EDITOR
    IReadOnlyList<Type> ComponentTypes { get; }
#endif
  }
}