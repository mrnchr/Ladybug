using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsRawEntities
  {
    private readonly List<EcsPackedEntity> _packedEntities;
    private readonly EcsEntityWrapper _cachedEntityWrapper;

    private EcsWorld _world;

    public List<EcsPackedEntity> Entities => _packedEntities;

    public EcsWorld World => _world;

    public EcsRawEntities(List<EcsPackedEntity> packedEntities, EcsWorld world)
    {
      _packedEntities = packedEntities;
      _world = world;
      _cachedEntityWrapper = new EcsEntityWrapper(_world);
    }

    public EcsRawEntities(EcsWorld world) : this(new List<EcsPackedEntity>(), world)
    {
    }

    public EcsRawEntities()
    {
      _packedEntities = new List<EcsPackedEntity>();
      _cachedEntityWrapper = new EcsEntityWrapper();
    }

    public void AddRaw(int entity)
    {
      _packedEntities.Add(_world.PackEntity(entity));
    }

    public void SetWorld(EcsWorld world)
    {
      _world = world;
      _cachedEntityWrapper.SetWorld(world);
    }

    public Enumerator GetEnumerator()
    {
      return new Enumerator(this);
    }

    public bool Any()
    {
      using Enumerator enumerator = GetEnumerator();
      return enumerator.MoveNext();
    }

    public IEnumerable<EcsEntityWrapper> ToEnumerable()
    {
      using Enumerator enumerator = GetEnumerator();
      while (enumerator.MoveNext())
        yield return enumerator.Current;
    }

    public EcsRawEntities Clone()
    {
      var instance = new EcsRawEntities(_packedEntities, _world);
      return instance;
    }

    public struct Enumerator : IDisposable
    {
      private readonly EcsRawEntities _entities;
      private List<EcsPackedEntity>.Enumerator _enumerator;

      private int _current;

      public Enumerator(EcsRawEntities entities)
      {
        _entities = entities;
        _enumerator = entities._packedEntities.GetEnumerator();
        _current = -1;
      }

      public EcsEntityWrapper Current
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _entities._cachedEntityWrapper;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
        while (_enumerator.MoveNext())
        {
          _enumerator.Current.Unpack(_entities._world, out _current);
          if (_current == -1)
            continue;

          _entities._cachedEntityWrapper.Entity = _current;
          return true;
        }

        return false;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void Dispose()
      {
        _enumerator.Dispose();
      }
    }
  }
}