﻿using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.VirtualCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Canvas;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class UpdateFeature : EcsFeature
  {
    public UpdateFeature(IEcsSystemFactory systems)
    {
      Add(systems.Create<CreationFeature>());
      Add(systems.Create<VirtualCameraFeature>());
      Add(systems.Create<CameraFeature>());
      Add(systems.Create<CanvasFeature>());
    }    
  }
}