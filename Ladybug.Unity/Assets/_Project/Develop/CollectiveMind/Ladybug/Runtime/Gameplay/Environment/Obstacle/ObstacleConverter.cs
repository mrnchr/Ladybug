using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay.Signal;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using JetBrains.Annotations;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [UsedImplicitly]
  public class ObstacleConverter : ISerializedEcsConverter
  {
    [SerializeField]
    private EntityType _entityId;

    [SerializeField]
    private SignalType _signalType;

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity
        .Replace((ref EntityId entityId) => entityId.Id = _entityId)
        .Has<ObstacleTag>(true)
        .Has<Destroyable>(true)
        .Has<Cleanable>(true)
        .Replace((ref SignalAssigment signalAssigment) => signalAssigment.SignalType = _signalType);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity
        .Has<EntityId>(false)
        .Has<ObstacleTag>(false)
        .Has<Destroyable>(false)
        .Has<Cleanable>(false)
        .Has<SignalAssigment>(false);
    }

#if UNITY_EDITOR
    private static readonly List<Type> _componentTypes = new List<Type>
    {
      typeof(EntityId),
      typeof(ObstacleTag),
      typeof(Destroyable),
      typeof(Cleanable),
      typeof(SignalAssigment)
    };

    public IReadOnlyList<Type> ComponentTypes => _componentTypes;
#endif
  }
}