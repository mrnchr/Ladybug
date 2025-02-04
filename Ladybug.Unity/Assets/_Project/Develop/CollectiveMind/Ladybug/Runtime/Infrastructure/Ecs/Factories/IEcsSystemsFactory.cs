using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsSystemsFactory
  {
    EcsSystems Create(string defaultName);
  }
}