using System.Collections.Generic;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public interface ICollisionFiller
  {
    void Fill(OneSideCollision collision);
    void Flush(bool isFixed, List<OneSideCollision> result);
  }
}