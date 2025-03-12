using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class DeleteCollisionSystem : IEcsRunSystem
  {
    private readonly EcsWorld _message;
    private readonly EcsEntities _collisions;

    public DeleteCollisionSystem(MessageWorldWrapper messageWorldWrapper)
    {
      _message = messageWorldWrapper.World;

      _collisions = _message
        .Filter<CollisionMessage>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper col in _collisions)
        col.Dispose();
    }
  }
}