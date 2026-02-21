using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Closures
{
  public class Closure<TType>
  {
    public Predicate<TType> Predicate;
  }
}