using System.Collections.Generic;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure
{
  public static class ListEmpty<T>
  {
      public static readonly IReadOnlyList<T> Value = new List<T>(0);
  }
}