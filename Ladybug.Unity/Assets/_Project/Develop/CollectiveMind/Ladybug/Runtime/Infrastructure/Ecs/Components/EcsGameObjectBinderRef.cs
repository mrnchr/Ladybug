using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct EcsGameObjectBinderRef : IEcsComponent
  {
    public EcsGameObjectBinder Binder;
  }
}