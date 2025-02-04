using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
#if ENABLE_IL2CPP
  using Unity.IL2CPP.CompilerServices;
    
  [Il2CppSetOption(Option.NullChecks, false)]
  [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct ConverterRef : IEcsComponent
  {
    public IGameObjectConverter Converter;
  }
}