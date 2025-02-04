using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsSystemFactory
  {
    IEcsSystem Create<TSystem>() where TSystem : IEcsSystem;
  }
}