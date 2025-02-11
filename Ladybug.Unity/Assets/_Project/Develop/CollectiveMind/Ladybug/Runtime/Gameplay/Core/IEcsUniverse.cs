using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public interface IEcsUniverse
  {
    IEcsWorldWrapper GameWrapper { get; }
    IEcsWorldWrapper MessageWrapper { get; }
    EcsWorld Game { get; }
    EcsWorld Message { get; }
    EcsWorld.Mask FilterGame<TComponent>() where TComponent : struct;
    EcsWorld.Mask FilterMessage<TComponent>() where TComponent : struct;
  }
}