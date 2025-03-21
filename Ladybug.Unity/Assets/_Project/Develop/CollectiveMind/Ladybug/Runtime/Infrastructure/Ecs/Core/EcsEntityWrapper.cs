﻿using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public delegate void ActionRef<TComponent>(ref TComponent component);

  public class EcsEntityWrapper : IDisposable
  {
    private int _entity = -1;
    private EcsWorld _world;

    public EcsWorld World => _world;

    public int Entity
    {
      get => _entity;
      set
      {
        _entity = value;

        PackedEntity = _world != null && _entity != -1
          ? _world.PackEntity(_entity)
          : new EcsPackedEntity();
      }
    }

    public EcsPackedEntity PackedEntity { get; private set; }

    public EcsEntityWrapper()
    {
    }

    public EcsEntityWrapper(EcsWorld world, int entity = -1)
    {
      SetWorld(world, entity);
    }

    public void Copy(EcsEntityWrapper from)
    {
      SetWorld(from.World, from.Entity);
    }

    public bool IsAlive()
    {
      return _world != null && PackedEntity.Unpack(_world, out int entity) && (Entity = entity) != -1;
    }

    public void SetWorld(EcsWorld world, int entity = -1)
    {
      _world = world;
      Entity = entity;
    }

    [HideInCallstack]
    public EcsEntityWrapper Add<TComponent>() where TComponent : struct, IEcsComponent
    {
      World.Add<TComponent>(Entity);
      return this;
    }

    [HideInCallstack]
    public EcsEntityWrapper Add<TComponent>(TComponent component) where TComponent : struct, IEcsComponent
    {
      ref TComponent addedComponent = ref World.Add<TComponent>(Entity);
      addedComponent = component;
      return this;
    }

    [HideInCallstack]
    public EcsEntityWrapper Add<TComponent>(ActionRef<TComponent> assigner) where TComponent : struct, IEcsComponent
    {
      ref TComponent component = ref World.Add<TComponent>(Entity);
      assigner.Invoke(ref component);
      return this;
    }

    [HideInCallstack]
    public ref TComponent AddOrGet<TComponent>() where TComponent : struct, IEcsComponent
    {
      if (!Has<TComponent>())
        Add<TComponent>();

      return ref Get<TComponent>();
    }

    [HideInCallstack]
    public EcsEntityWrapper Change<TComponent>(TComponent component) where TComponent : struct, IEcsComponent
    {
      ref TComponent refComponent = ref World.Get<TComponent>(Entity);
      refComponent = component;
      return this;
    }

    [HideInCallstack]
    public EcsEntityWrapper Change<TComponent>(ActionRef<TComponent> replacer) where TComponent : struct, IEcsComponent
    {
      ref TComponent component = ref World.Get<TComponent>(Entity);
      replacer.Invoke(ref component);
      return this;
    }

    [HideInCallstack]
    public EcsEntityWrapper Del<TComponent>() where TComponent : struct, IEcsComponent
    {
      World.Del<TComponent>(Entity);
      return this;
    }

    [HideInCallstack]
    public ref TComponent Get<TComponent>() where TComponent : struct, IEcsComponent
    {
      return ref World.Get<TComponent>(Entity);
    }

    [HideInCallstack]
    public bool Has<TComponent>() where TComponent : struct, IEcsComponent
    {
      return World.Has<TComponent>(Entity);
    }

    [HideInCallstack]
    public EcsEntityWrapper Has<TComponent>(bool value) where TComponent : struct, IEcsComponent
    {
      switch (value, World.Has<TComponent>(Entity))
      {
        case (true, false):
          Add<TComponent>();
          break;
        case (false, true):
          Del<TComponent>();
          break;
      }

      return this;
    }

    [HideInCallstack]
    public EcsEntityWrapper Replace<TComponent>(TComponent component) where TComponent : struct, IEcsComponent
    {
      ref TComponent refComponent = ref AddOrGet<TComponent>();
      refComponent = component;
      return this;
    }

    [HideInCallstack]
    public EcsEntityWrapper Replace<TComponent>(ActionRef<TComponent> replacer) where TComponent : struct, IEcsComponent
    {
      ref TComponent component = ref AddOrGet<TComponent>();
      replacer.Invoke(ref component);
      return this;
    }

    public void Dispose()
    {
      World.DelEntity(Entity);
      Entity = -1;
    }

    public EcsPackedEntityWithWorld PackWithWorld()
    {
      return World.PackEntityWithWorld(Entity);
    }
  }
}