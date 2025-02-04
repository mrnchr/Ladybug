using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsPredicate
  {
    public bool Invoke(int entity);
    Type ComponentType { get; }
  }
}