using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint.Components;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint
{
  public class SpawnPointGizmo : MonoBehaviour
  {
#if UNITY_EDITOR
    private GameObjectConverter _converter;
    private void OnDrawGizmos()
    {
      if (!_converter)
        _converter = GetComponent<GameObjectConverter>();
      if (!_converter)
        return;
      var converters = (List<EcsConverterValue>)_converter.GetType()
        .GetField("_converters", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(_converter);
      if (converters == null)
        return;
      
      EntityType id = converters.Where(x => x.ShowComponents)
        .Select(x => (x.GetValue() as EcsComponentsConverter)?.Components.Select(y => y.Value)
          .OfType<SpawnedEntityId>()
          .FirstOrDefault()
          .Id ?? EntityType.None).FirstOrDefault();

      if (id == EntityType.None)
        return;
      
      Color color = id == EntityType.Ladybug ? Color.green : Color.red;
      color.a = 0.75f;
      Gizmos.color = color;
      Gizmos.DrawSphere(transform.position, 1f);
    }
#endif
  }
}