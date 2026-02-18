using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct FacadeRef : IEcsComponent
  {
    public IFacade Facade;
  }
}