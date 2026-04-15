using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  public class EcsConverterValue : IEcsConverter
  {
    [SerializeReference]
    [ShowIf(nameof(ShowSerializedConverter))]
    [LabelText("$" + nameof(_serializedConverter) + "Name")]
    private ISerializedEcsConverter _serializedConverter;

    [SerializeField]
    [ShowIf(nameof(HasComponents))]
    [InlineProperty]
    [HideLabel]
    private EcsComponentsConverter _components;

    [SerializeField]
    [ShowIf(nameof(ShowScriptableConverter))]
    [InlineEditor]
    [HideLabel]
    [LabelText("$" + nameof(_scriptableConverter) + "Name")]
    [InfoBox("Edit data within this field with care", TriMessageType.Warning, "$" + nameof(HasScriptableConverter))]
    private EcsConverterAsset _scriptableConverter;

    public bool IsEmpty => _serializedConverter == null
      && !HasComponents
      && _scriptableConverter == null;

    public bool ShowSerializedConverter => IsEmpty || _serializedConverter != null;
    public bool HasComponents => _components.Components.Count > 0;
    public bool ShowScriptableConverter => IsEmpty || _scriptableConverter;
    public bool HasScriptableConverter => _scriptableConverter;

    public IEcsConverter GetValue()
    {
      if (_serializedConverter != null) return _serializedConverter;
      if (HasComponents) return _components;
      if (_scriptableConverter) return _scriptableConverter;

      return null;
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      IEcsConverter value = GetValue();
      value?.ConvertTo(entity);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
    }

#if UNITY_EDITOR
    private string _serializedConverterName => UnityEditor.ObjectNames.NicifyVariableName(_serializedConverter?.GetType().Name ?? TriConstants.NONE);
    private string _scriptableConverterName => UnityEditor.ObjectNames.NicifyVariableName(_scriptableConverter ? _scriptableConverter.name : TriConstants.NONE);

    [Button("Clear")]
    [GUIColor(CC.RED)]
    [PropertyOrder(0)]
    [HideIf("$" + nameof(IsEmpty))]
    private void Clear()
    {
      _serializedConverter = null;
      _components.Components.Clear();
      _scriptableConverter = null;
    }

    [Button]
    [GUIColor(CC.CYAN)]
    [PropertyOrder(1)]
    [ShowIf("$" + nameof(IsEmpty))]
    private void CreateComponentList()
    {
      _components.Components.Add(new EcsComponentValue());
    }
    
    public IReadOnlyList<Type> ComponentTypes
    {
      get
      {
        if (_serializedConverter != null) return _serializedConverter.ComponentTypes;
        if (HasComponents) return _components.ComponentTypes;
        if (_scriptableConverter) return _scriptableConverter.ComponentTypes;

        return ListEmpty<Type>.Value;
      }
    }
#endif
  }
}