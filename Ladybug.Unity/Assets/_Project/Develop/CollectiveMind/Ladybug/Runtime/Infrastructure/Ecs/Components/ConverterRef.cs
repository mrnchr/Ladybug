using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct ConverterRef : IEcsComponent
  {
    public IGameObjectConverter Converter;
  }
}