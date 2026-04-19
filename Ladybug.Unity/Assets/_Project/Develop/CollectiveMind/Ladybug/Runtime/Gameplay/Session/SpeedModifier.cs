using System;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Session
{
  public class SpeedModifier : IDisposable
  {
    public event Action OnDispose;

    private readonly SpeedModifierType _type;
    private readonly float _value;
    private bool _disposed;

    public SpeedModifier(SpeedModifierType type, float value)
    {
      if (type == SpeedModifierType.Divide && Mathf.Approximately(value, 0f))
        Debug.LogError($"[{nameof(SpeedModifier)}] Modifier with divide by zero operation");
      
      _type = type;
      _value = value;
    }
    
    public float Apply(float speed)
    {
      return _type switch
      {
        SpeedModifierType.Add => speed + _value,
        SpeedModifierType.Subtract => speed - _value,
        SpeedModifierType.Multiply => speed * _value,
        SpeedModifierType.Divide => Mathf.Approximately(_value, 0f) ? speed : speed / _value,
        _ => throw new ArgumentOutOfRangeException(nameof(_type))
      };
    }
    
    public void Dispose()
    {
      if (_disposed)
        return;

      _disposed = true;
      OnDispose?.Invoke();
    }
  }
}