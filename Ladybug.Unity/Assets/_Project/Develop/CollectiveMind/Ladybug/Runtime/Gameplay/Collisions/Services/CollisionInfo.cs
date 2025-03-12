using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class CollisionInfo
  {
    public PackedCollider MasterCollider;
    public PackedCollider TargetCollider;

    public EcsPackedEntity PackedMaster => MasterCollider.Entity;
    public EcsPackedEntity PackedTarget => TargetCollider.Entity;

    public EcsEntityWrapper Master;
    public EcsEntityWrapper Target;

    public void Reset()
    {
      MasterCollider = new PackedCollider();
      TargetCollider = new PackedCollider();
      Master = null;
      Target = null;
    }
  }
}