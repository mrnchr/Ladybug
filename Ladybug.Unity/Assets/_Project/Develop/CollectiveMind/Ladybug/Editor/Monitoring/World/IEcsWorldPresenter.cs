using System.Collections.Generic;
using CollectiveMind.Ladybug.Editor.Monitoring.Entity;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Editor.Monitoring.World
{
  public interface IEcsWorldPresenter
  {
    EcsWorldView View { get; }
    List<IEcsEntityPresenter> Children { get; }
    IEcsWorldWrapper Wrapper { get; }
    IEcsPool[] Pools { get; }
    void Initialize();
    void Tick();
  }
}