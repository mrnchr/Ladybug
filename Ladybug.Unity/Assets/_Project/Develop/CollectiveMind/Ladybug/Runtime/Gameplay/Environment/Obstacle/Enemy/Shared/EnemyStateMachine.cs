using System;
using System.Collections.Generic;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy
{
  public class EnemyStateMachine : IGameStep
  {
    private readonly EnemyFacade _owner;
    private readonly Dictionary<Type, BaseEnemyState> _states;

    private BaseEnemyState _currentState;

    public EnemyStateMachine(EnemyFacade owner, List<Type> stateTypes)
    {
      _owner = owner;

      if (stateTypes == null || stateTypes.Count == 0)
        Debug.LogError($"[{nameof(EnemyStateMachine)}] Trying to initialize with empty array");

      foreach (var type in stateTypes)
      {
        var state = CreateState(type);
        AddState(state);
      }
    }

    public void AddState(BaseEnemyState state)
    {
      if (state == null)
        Debug.LogWarning($"[{nameof(EnemyStateMachine)}] Trying to add null state");
      if (!_states.TryAdd(state.GetType(), state))
        Debug.LogWarning($"[{nameof(EnemyStateMachine)}] Trying to add multiple states of type: {state.GetType()}");
    }

    public void RemoveState<T>() where T : BaseEnemyState
    {
      _states.Remove(typeof(T));
    }

    public void Step()
    {
      _currentState?.Step();
    }

    public bool Switch<T>(BaseEnemyStateData data = null) where T : BaseEnemyState
    {
      Type stateType = typeof(T);
      return Switch(stateType, data);
    }

    public bool Switch(Type stateType, BaseEnemyStateData data = null)
    {
      if (!_states.TryGetValue(stateType, out var targetState))
      {
        Debug.LogWarning($"[{nameof(EnemyStateMachine)}] State not found: {stateType.Name}");
        return false;
      }

      _currentState?.Exit();
      targetState.Enter(data);
      _currentState = targetState;

      return true;
    }

    private BaseEnemyState CreateState(Type type)
    {
      try
      {
        return Activator.CreateInstance(type, this, _owner) as BaseEnemyState;
      }
      catch (Exception e)
      {
        Debug.LogError($"[{nameof(EnemyStateMachine)}] {e}");
      }
      
      return null;
    }
  }
}