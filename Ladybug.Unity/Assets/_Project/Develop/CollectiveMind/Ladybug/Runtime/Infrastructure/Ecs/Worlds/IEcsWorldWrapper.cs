using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds
{
  public interface IEcsWorldWrapper
  {
    string Name { get; }
    EcsWorld World { get; }
  }
}