using System;
using TriInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  [InlineProperty]
  public class EcsComponentValue
  {
    [LabelText("$Name")]
    [SerializeReference]
    [HideLabel]
    [OnValueChanged(TriConstants.ON + nameof(Value) + TriConstants.CHANGED)]
    public IEcsComponent Value;

    private Type _type;

    public Type Type => _type ??= Value?.GetType();

#if UNITY_EDITOR
    private string Name => EcsComponentNameResolver.GetComponentName(Value?.GetType());
    
    private void OnValueChanged()
    {
      if (EditorApplication.isPlaying)
        _type = Value?.GetType();
    }
#endif
  }
}