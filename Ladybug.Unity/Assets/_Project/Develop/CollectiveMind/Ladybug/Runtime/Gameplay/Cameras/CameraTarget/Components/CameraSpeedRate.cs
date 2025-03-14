using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  [Serializable]
  public struct CameraSpeedRate : IEcsComponent
  {
    public float Rate;
  }
}