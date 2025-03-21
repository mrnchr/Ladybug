﻿using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class CollisionFilter : ICollisionFilter
  {
    public CollisionInfo Info { get; } = new CollisionInfo();

    public void AssignCollision(TwoSideCollision collision)
    {
      Info.Reset();
      Info.MasterCollider = collision.Sender;
      Info.TargetCollider = collision.Other;
    }

    public void AssignCollision(OneSideCollision collision)
    {
      Info.Reset();
      Info.MasterCollider = collision.Sender;
      Info.TargetCollider.Collider = collision.Other;
    }

    public bool TrySelectByMasterCollider(Predicate<PackedCollider> selector, bool sync = true)
    {
      var selection = new List<PackedCollider> { Info.MasterCollider, Info.TargetCollider };
      return TrySelect(() => SelectByMaster(() => selection.FindIndex(selector)),
        AssignColliders(selection),
        sync, () => SyncEntitiesWithColliders());
    }

    public bool TrySelectByTargetCollider(Predicate<PackedCollider> selector, bool sync = true)
    {
      var selection = new List<PackedCollider> { Info.MasterCollider, Info.TargetCollider };
      return TrySelect(() => SelectByTarget(() => selection.FindIndex(selector)),
        AssignColliders(selection),
        sync, () => SyncEntitiesWithColliders());
    }

    public bool TrySelectByColliders(Predicate<PackedCollider> masterSelector, Predicate<PackedCollider> targetSelector,
      bool sync = true)
    {
      var selection = new List<PackedCollider> { Info.MasterCollider, Info.TargetCollider };
      return TrySelect(
        () => SelectBoth(() => selection.FindIndex(masterSelector), () => selection.FindIndex(targetSelector)),
        AssignColliders(selection),
        sync, () => SyncEntitiesWithColliders());
    }

    public bool TrySelectByMasterEntity(Predicate<EcsEntityWrapper> selector, bool sync = true)
    {
      var selection = new List<EcsEntityWrapper> { Info.Master, Info.Target };
      return TrySelect(() => SelectByMaster(() => selection.FindIndex(selector)),
        AssignEntities(selection),
        sync, () => SyncCollidersWithEntities());
    }

    public bool TrySelectByTargetEntity(Predicate<EcsEntityWrapper> selector, bool sync = true)
    {
      var selection = new List<EcsEntityWrapper> { Info.Master, Info.Target };
      return TrySelect(() => SelectByTarget(() => selection.FindIndex(selector)),
        AssignEntities(selection),
        sync, () => SyncCollidersWithEntities());
    }

    public bool TrySelectByEntities(Predicate<EcsEntityWrapper> masterSelector, Predicate<EcsEntityWrapper> targetSelector,
      bool sync = true)
    {
      var selection = new List<EcsEntityWrapper> { Info.Master, Info.Target };
      return TrySelect(
        () => SelectBoth(() => selection.FindIndex(masterSelector), () => selection.FindIndex(targetSelector)),
        AssignEntities(selection),
        sync, () => SyncCollidersWithEntities());
    }

    public bool SyncCollidersWithEntities()
    {
      return TrySelectByColliders(x => x.Entity.EqualsTo(Info.Master.PackedEntity),
        x => x.Entity.EqualsTo(Info.Target.PackedEntity), false);
    }

    public bool SyncEntitiesWithColliders()
    {
      return TrySelectByEntities(x => Info.MasterCollider.Entity.EqualsTo(x.PackedEntity),
        x => Info.TargetCollider.Entity.EqualsTo(x.PackedEntity), false);
    }

    public bool TryUnpackBothEntities(EcsWorld world)
    {
      return Info.MasterCollider.Entity.TryUnpackEntity(world, out Info.Master)
        & Info.TargetCollider.Entity.TryUnpackEntity(world, out Info.Target);
    }

    public bool TryUnpackByMasterEntity(EcsWorld world)
    {
      Info.TargetCollider.Entity.TryUnpackEntity(world, out Info.Target);
      return Info.MasterCollider.Entity.TryUnpackEntity(world, out Info.Master);
    }

    public bool TryUnpackByTargetEntity(EcsWorld world)
    {
      Info.MasterCollider.Entity.TryUnpackEntity(world, out Info.Master);
      return Info.TargetCollider.Entity.TryUnpackEntity(world, out Info.Target);
    }
    
    public bool TryUnpackAnyEntity(EcsWorld world)
    {
      return Info.MasterCollider.Entity.TryUnpackEntity(world, out Info.Master)
        | Info.TargetCollider.Entity.TryUnpackEntity(world, out Info.Target);
    }

    public bool UnpackEntities(EcsWorld world)
    {
      Info.MasterCollider.Entity.TryUnpackEntity(world, out Info.Master);
      Info.TargetCollider.Entity.TryUnpackEntity(world, out Info.Target);
      return true;
    }

    private Action<int, int> AssignColliders(List<PackedCollider> selection)
    {
      return (master, target) =>
      {
        Info.MasterCollider = selection[master];
        Info.TargetCollider = selection[target];
      };
    }

    private Action<int, int> AssignEntities(List<EcsEntityWrapper> selection)
    {
      return (master, target) =>
      {
        Info.Master = selection[master];
        Info.Target = selection[target];
      };
    }

    private (int, int) SelectByMaster(Func<int> selector)
    {
      int masterIndex = selector();
      int targetIndex = (masterIndex + 1) % 2;

      return (masterIndex, targetIndex);
    }

    private (int, int) SelectByTarget(Func<int> selector)
    {
      int targetIndex = selector();
      int masterIndex = (targetIndex + 1) % 2;

      return (masterIndex, targetIndex);
    }

    private (int, int) SelectBoth(Func<int> masterSelector, Func<int> targetSelector)
    {
      int masterIndex = masterSelector();
      int targetIndex = targetSelector();

      return (masterIndex, targetIndex);
    }

    private bool TrySelect(Func<(int master, int target)> selector, Action<int, int> assigner, bool sync, Action syncer)
    {
      return TrySelectInternal(selector, assigner, sync ? syncer : null);
    }

    private bool TrySelectInternal(Func<(int master, int target)> selector, Action<int, int> assigner,
      Action syncer = null)
    {
      (int master, int target) = selector();
      if (!TryAssign(assigner, master, target))
        return false;

      syncer?.Invoke();
      return true;
    }


    private static bool TryAssign(Action<int, int> assigner, int masterIndex, int targetIndex)
    {
      if (masterIndex == -1 || targetIndex == -1 || masterIndex == targetIndex)
        return false;

      assigner(masterIndex, targetIndex);

      return true;
    }
  }
}