using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsWorldWrapper
  {
    string Name { get; }
    EcsWorld World { get; }
  }
}