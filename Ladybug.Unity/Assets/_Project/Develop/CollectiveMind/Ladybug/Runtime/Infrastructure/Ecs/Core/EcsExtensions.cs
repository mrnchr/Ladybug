﻿using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public static class EcsExtensions
  {
    [HideInCallstack]
    public static ref TComponent Add<TComponent>(this EcsWorld world, int entity)
      where TComponent : struct, IEcsComponent
    {
      return ref world.GetPool<TComponent>().Add(entity);
    }

    [HideInCallstack]
    public static ref TComponent Get<TComponent>(this EcsWorld world, int entity)
      where TComponent : struct, IEcsComponent
    {
      return ref world.GetPool<TComponent>().Get(entity);
    }

    [HideInCallstack]
    public static bool Has<TComponent>(this EcsWorld world, int entity) where TComponent : struct, IEcsComponent
    {
      return world.GetPool<TComponent>().Has(entity);
    }

    [HideInCallstack]
    public static void Del<TComponent>(this EcsWorld world, int entity) where TComponent : struct, IEcsComponent
    {
      world.GetPool<TComponent>().Del(entity);
    }

    [HideInCallstack]
    public static IEnumerable<int> Where<TComponent>(this EcsFilter filter, Predicate<TComponent> predicate)
      where TComponent : struct, IEcsComponent
    {
      EcsPool<TComponent> pool = filter.GetWorld().GetPool<TComponent>();
      var selection = new List<int>();
      foreach (int i in filter)
        if (predicate.Invoke(pool.Get(i)))
          selection.Add(i);

      return selection;
    }

    [HideInCallstack]
    public static EcsEntities Collect(this EcsWorld.Mask mask)
    {
      return new EcsEntities(mask.End());
    }

    [HideInCallstack]
    public static EcsEntityWrapper CreateEntity(this EcsWorld world)
    {
      return new EcsEntityWrapper(world, world.NewEntity());
    }

    public static bool TryUnpackEntity(this EcsPackedEntity packedEntity, EcsWorld world, out EcsEntityWrapper entity)
    {
      bool packed = packedEntity.Unpack(world, out int ent);
      entity = new EcsEntityWrapper(world, ent);
      return packed;
    }

    public static bool TryUnpackEntity(this EcsPackedEntity packedEntity, EcsWorld world, EcsEntityWrapper entity)
    {
      bool packed = packedEntity.Unpack(world, out int ent);
      entity.SetWorld(world, ent);
      return packed;
    }
  }
}