using System.Collections.Generic;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class CollisionFiller : ICollisionFiller
  {
    private readonly List<OneSideCollision> _fixedCollisions = new List<OneSideCollision>();
    private readonly List<OneSideCollision> _collisions = new List<OneSideCollision>();

    public void Fill(OneSideCollision collision)
    {
      _fixedCollisions.Add(collision);
      _collisions.Add(collision);
    }

    public void Flush(bool isFixed, List<OneSideCollision> result)
    {
      List<OneSideCollision> collisions = isFixed ? _fixedCollisions : _collisions;
      result.AddRange(collisions);
      collisions.Clear();
    }
  }
}