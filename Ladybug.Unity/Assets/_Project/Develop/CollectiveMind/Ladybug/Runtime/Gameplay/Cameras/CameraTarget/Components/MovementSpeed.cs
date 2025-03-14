using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct MovementSpeed : IEcsComponent
  {
    public float Speed;
  }
}