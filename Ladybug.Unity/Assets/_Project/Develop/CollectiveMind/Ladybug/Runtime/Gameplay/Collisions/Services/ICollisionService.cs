using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public interface ICollisionService
  {
    CollisionInfo Info { get; }
    void AssignCollision(TwoSideCollision collision);
    void AssignCollision(OneSideCollision collision);
    bool TrySelectByMasterCollider(Predicate<PackedCollider> selector, bool sync = true);
    bool TrySelectByTargetCollider(Predicate<PackedCollider> selector, bool sync = true);

    bool TrySelectByColliders(Predicate<PackedCollider> masterSelector, Predicate<PackedCollider> targetSelector,
      bool sync = true);

    bool TrySelectByMasterEntity(Predicate<EcsEntityWrapper> selector, bool sync = true);
    bool TrySelectByTargetEntity(Predicate<EcsEntityWrapper> selector, bool sync = true);
    bool TrySelectByEntities(Predicate<EcsEntityWrapper> masterSelector, Predicate<EcsEntityWrapper> targetSelector, bool sync = true);
    bool SyncCollidersWithEntities();
    bool SyncEntitiesWithColliders();
    bool TryUnpackBothEntities(EcsWorld world);
    bool UnpackEntities(EcsWorld world);
    bool TryUnpackByMasterEntity(EcsWorld world);
    bool TryUnpackByTargetEntity(EcsWorld world);
    bool TryUnpackAnyEntity(EcsWorld world);
  }
}