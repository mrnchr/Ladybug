using System.Collections.Generic;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsGameObjectBinder : MonoBehaviour
  {
    private readonly List<IEcsConverter> _componentConverters = new List<IEcsConverter>();
    
    private EcsEntityWrapper _entity;

    public void Bind(EcsEntityWrapper entity)
    {
      _entity = entity;
      ConvertTo(_entity);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      foreach (IEcsConverter converter in _componentConverters)
        converter.ConvertBack(entity);

      ConvertBackDefaultComponents(entity);
    }

    private void OnDestroy()
    {
      if (_entity.IsAlive() && _entity.Has<GameObjectRef>())
      {
        ConvertBack(_entity);
      }
    }

    private void CollectConvertersInHierarchy()
    {
      _componentConverters.Clear();
      _componentConverters.AddRange(GetComponents<IEcsConverter>());

      for (int i = 0; i < transform.childCount; i++)
      {
        CollectConvertersRecursive(transform.GetChild(i), _componentConverters);
      }
    }

    private void CollectConvertersRecursive(Transform t, List<IEcsConverter> converters)
    {
      IEcsConverter[] list = t.GetComponents<IEcsConverter>();

      converters.AddRange(list);
      for (int i = 0; i < t.childCount; i++)
        CollectConvertersRecursive(t.GetChild(i), converters);
    }

    private void ConvertTo(EcsEntityWrapper entity)
    {
      CollectConvertersInHierarchy();
      
      foreach (IEcsConverter converter in _componentConverters)
        converter.ConvertTo(entity);

      ConvertDefaultComponents(entity);
    }

    private void ConvertDefaultComponents(EcsEntityWrapper entity)
    {
      entity
        .Add((ref GameObjectRef gameObjectRef) => gameObjectRef.GameObject = gameObject)
        .Add((ref TransformRef transformRef) => transformRef.Transform = transform)
        .Add((ref EcsGameObjectBinderRef converterRef) => converterRef.Binder = this);
    }

    private void ConvertBackDefaultComponents(EcsEntityWrapper entity)
    {
      entity
        .Del<EcsGameObjectBinderRef>()
        .Del<TransformRef>()
        .Del<GameObjectRef>();
    }
  }
}