using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [CreateAssetMenu(menuName = CAC.Names.ECS_CONVERTER_MENU, fileName = CAC.Names.ECS_CONVERTER_FILE)]
  [HideMonoScript]
  public class EcsConverterAsset : ScriptableObject, IEcsConverter
  {
    public List<EcsConverterValue> Converters;

    public void ConvertTo(EcsEntityWrapper entity)
    {
      foreach (EcsConverterValue converter in Converters)
      {
        converter.ConvertTo(entity);
      }
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium)]
    [PropertyOrder(0)]
    [GUIColor(0f, 1f, 1f)]
    private void ValidateEntity()
    {
      var world = new EcsWorld();
      EcsEntityWrapper entity = world.CreateEntity();
      try
      {
        ConvertTo(entity);
        Debug.Log("Entity was successfully created");
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }
    }

    [Button(ButtonSizes.Medium)]
    [PropertyOrder(0)]
    [GUIColor(CC.CYAN)]
    private void ShowOverridenComponents()
    {
      int count = 0;
      var componentTypes = new HashSet<Type>();

      foreach ((EcsConverterAsset Asset, Type Type) componentType in _overridenComponents)
      {
        if (!componentTypes.Add(componentType.Type))
        {
          Debug.LogWarning(
            $"[{EcsComponentNameResolver.GetComponentName(componentType.Type)}] component is overriden in [{componentType.Asset.name}]",
            componentType.Asset);
          count++;
        }
      }

      Debug.Log(count != 0 ? $"Found {count} overriden components" : "No overriden components");
    }
    
    public IReadOnlyList<Type> ComponentTypes
    {
      get
      {
        _componentTypes.Clear();
        
        foreach (EcsConverterValue converterValue in Converters)
        {
          _componentTypes.AddRange(converterValue.ComponentTypes);
        }

        return _componentTypes;
      }
    }

    private IReadOnlyList<(EcsConverterAsset, Type)> _overridenComponents
    {
      get
      {
        var componentTypes = new List<(EcsConverterAsset, Type)>();
        
        foreach (EcsConverterValue converterValue in Converters)
        {
          if (converterValue.ShowScriptableConverter && converterValue.GetValue() is EcsConverterAsset asset)
          {
            componentTypes.AddRange(asset._overridenComponents);
          }
          else
          {
            componentTypes.AddRange(converterValue.ComponentTypes.Select<Type, (EcsConverterAsset, Type)>(x => (this, x)));
          }
        }

        return componentTypes;
      }
    }

    private readonly List<Type> _componentTypes = new List<Type>();
#endif
  }
}