using System;
using Leopotam.EcsLite;
using TriInspector;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.TEMPORARY)]
  public struct CreateEntityMessage : IEcsComponent
  {
    [ShowInInspector]
    public EcsPackedEntity Entity;
    public IGameObjectConverter Converter;
  }
}