﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Closures;
using CollectiveMind.Ladybug.Runtime.Utils;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsEntities
  {
    private readonly List<IEcsPredicate> _predicates = new List<IEcsPredicate>();
    private readonly List<IEcsPredicate> _tempPredicates = new List<IEcsPredicate>();
    private readonly EcsFilter _filter;
    private readonly EcsWorld _world;
    private readonly EcsEntityWrapper _cachedEntityWrapper;
    private readonly SpecifiedClosure<IEcsPredicate, int> _invokePredicateClosure;

    public EcsFilter Filter => _filter;
    public EcsWorld World => _world;

    public EcsEntities(EcsFilter filter)
    {
      _filter = filter;
      _world = filter.GetWorld();
      _cachedEntityWrapper = new EcsEntityWrapper(_world);
      _invokePredicateClosure = new SpecifiedClosure<IEcsPredicate, int>((predicate, i) => predicate.Invoke(i));
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

    public EcsEntities Clone()
    {
      var instance = new EcsEntities(_filter);
      instance._predicates.AddRange(_predicates);
      return instance;
    }

    public EcsEntities Check<TComponent>(Predicate<TComponent> predicate) where TComponent : struct, IEcsComponent
    {
      EcsPredicate<TComponent> ecsPredicate = EcsPredicatePool.PopPredicate<TComponent>();
      ecsPredicate.Predicate = predicate;
      ecsPredicate.Pool = World.GetPool<TComponent>();
      _tempPredicates.Add(ecsPredicate);
      return this;
    }

    public EcsEntities Where<TComponent>(Predicate<TComponent> predicate) where TComponent : struct, IEcsComponent
    {
      _predicates.Add(new EcsPredicate<TComponent>(predicate, World.GetPool<TComponent>()));
      return this;
    }

    public struct Enumerator : IDisposable
    {
      private readonly EcsEntities _entities;
      private EcsFilter.Enumerator _enumerator;

      public Enumerator(EcsEntities entities)
      {
        _entities = entities;
        _enumerator = entities._filter.GetEnumerator();
      }

      public EcsEntityWrapper Current
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
          _entities._cachedEntityWrapper.Entity = _enumerator.Current;
          return _entities._cachedEntityWrapper;
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
        while (_enumerator.MoveNext())
        {
          _entities._invokePredicateClosure.Data = _enumerator.Current;
          if (_entities._predicates.AllNonAlloc(_entities._invokePredicateClosure.Predicate)
            && _entities._tempPredicates.AllNonAlloc(_entities._invokePredicateClosure.Predicate))
          {
            return true;
          }
        }

        return false;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void Dispose()
      {
        _enumerator.Dispose();
        foreach (IEcsPredicate predicate in _entities._tempPredicates)
          EcsPredicatePool.PushPredicate(predicate);

        _entities._tempPredicates.Clear();
      }
    }
  }
}