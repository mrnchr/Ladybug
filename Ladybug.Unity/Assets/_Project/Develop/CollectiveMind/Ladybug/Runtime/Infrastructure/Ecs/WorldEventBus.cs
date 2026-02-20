using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class WorldEventBus
  {
    private readonly Dictionary<Type, Dictionary<EcsPackedEntity, Action>> _subscribers =
      new Dictionary<Type, Dictionary<EcsPackedEntity, Action>>();

    public IDisposable Subscribe<T>(EcsPackedEntity packedEntity, Action action) where T : struct
    {
      Type type = typeof(T);
      
      if (!_subscribers.TryGetValue(type, out Dictionary<EcsPackedEntity, Action> dictionary))
      {
        dictionary = new Dictionary<EcsPackedEntity, Action>();
        dictionary.Add(packedEntity, action);
        _subscribers.Add(type, dictionary);
      }
      
      if (dictionary.TryGetValue(packedEntity, out Action existing))
      {
        dictionary[packedEntity] = existing + action;
      }
      else
      {
        dictionary.Add(packedEntity, action);
      }
      
      return new Unsubscriber<T>(this, packedEntity, action);
    }

    public void Publish<T>(EcsPackedEntity packedEntity) where T : struct
    {
      if (_subscribers.TryGetValue(typeof(T), out Dictionary<EcsPackedEntity, Action> dictionary)
        && dictionary.TryGetValue(packedEntity, out Action actions))
      {
        actions.Invoke();
      }
    }

    private void Unsubscribe<T>(EcsPackedEntity packedEntity, Action action) where T : struct
    {
      Type type = typeof(T);
      
      if (!_subscribers.TryGetValue(type, out Dictionary<EcsPackedEntity, Action> dictionary)
        || !dictionary.TryGetValue(packedEntity, out Action existing))
      {
        return;
      }

      existing -= action;

      if (existing == null)
      {
        dictionary.Remove(packedEntity);

        if (dictionary.Count == 0)
        {
          _subscribers.Remove(type);
        }
      }
      else
      {
        dictionary[packedEntity] = existing;
      }
    }

    private readonly struct Unsubscriber<T> : IDisposable where T : struct
    {
      private readonly WorldEventBus _bus;
      private readonly EcsPackedEntity _packedEntity;
      private readonly Action _action;

      public Unsubscriber(WorldEventBus bus, EcsPackedEntity packedEntity, Action action)
      {
        _bus = bus;
        _packedEntity = packedEntity;
        _action = action;
      }

      public void Dispose()
      {
        _bus.Unsubscribe<T>(_packedEntity, _action);
      }
    }
  }
}