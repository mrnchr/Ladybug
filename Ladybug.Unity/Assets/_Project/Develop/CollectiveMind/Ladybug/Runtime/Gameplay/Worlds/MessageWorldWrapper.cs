using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Worlds
{
  public class MessageWorldWrapper : IEcsWorldWrapper
  {
    private readonly EcsWorld _world = new EcsWorld();

    public string Name => EcsConstants.Worlds.MESSAGE;
    public EcsWorld World => _world;
  }
}