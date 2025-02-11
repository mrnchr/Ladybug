using CollectiveMind.Ladybug.Editor.Monitoring.Universe;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Editor.Monitoring.World
{
  public interface IEcsWorldPresenterFactory
  {
    IEcsWorldPresenter Create(IEcsWorldWrapper world, IEcsUniversePresenter parent);
  }
}