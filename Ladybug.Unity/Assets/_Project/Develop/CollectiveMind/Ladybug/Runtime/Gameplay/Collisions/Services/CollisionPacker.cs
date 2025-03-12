using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class CollisionPacker : ICollisionPacker
  {
    private readonly List<OneSideCollision> flushedCollisions = new List<OneSideCollision>();

    private readonly ICollisionFiller _filler;
    private readonly EcsWorld _message;

    public CollisionPacker(MessageWorldWrapper messageWorldWrapper, ICollisionFiller filler)
    {
      _filler = filler;
      _message = messageWorldWrapper.World;
    }

    public void Pack(bool isFixed = false)
    {
      flushedCollisions.Clear();
      _filler.Flush(isFixed, flushedCollisions);
      PackCollisions();
    }

    private void PackCollisions()
    {
      for (int i = 0; i < flushedCollisions.Count; i++)
      {
        OneSideCollision current = flushedCollisions[i];
        OneSideCollision other =
          flushedCollisions.Find(x => x.Type == current.Type && x.Sender.Collider == current.Other);
        if (!other.Other)
        {
          CreateOneSideCollisionMessage(current);
          continue;
        }

        CreateTwoSideCollisionMessage(new TwoSideCollision(current.Type, current.Sender, other.Sender));
        flushedCollisions.Remove(other);
      }
    }

    private void CreateOneSideCollisionMessage(OneSideCollision collision)
    {
      _message.CreateEntity()
        .Add<CollisionMessage>()
        .Add((ref OneSideCollision oneSideCollision) => oneSideCollision = collision);
    }

    private void CreateTwoSideCollisionMessage(TwoSideCollision collision)
    {
      _message.CreateEntity()
        .Add<CollisionMessage>()
        .Add((ref TwoSideCollision twoSideCollision) => twoSideCollision = collision);
    }
  }
}