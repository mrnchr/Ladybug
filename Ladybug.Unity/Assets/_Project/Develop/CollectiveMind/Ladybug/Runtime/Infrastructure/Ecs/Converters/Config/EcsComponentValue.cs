using System;
using TriInspector;
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
    
    public Type Type => _type ??= Value?.GetType();
    
    private Type _type;

#if UNITY_EDITOR
    public bool IsOverridenComponent { get; set; }
    private string Name => (IsOverridenComponent ? "[O] " : "") + EcsComponentNameResolver.GetComponentName(Value?.GetType());

    private void OnValueChanged()
    {
      _type = Value?.GetType();
    }
#endif
  }
}