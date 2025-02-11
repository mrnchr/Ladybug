﻿using CollectiveMind.Ladybug.Editor.Monitoring.World;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Entity
{
  public interface IEcsEntityPresenterFactory
  {
    IEcsEntityPresenter Create(int entity, IEcsWorldPresenter parent, IEcsWorldWrapper wrapper);
  }
}