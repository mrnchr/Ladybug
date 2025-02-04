using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using LudensClub.GeoChaos.Editor.Monitoring.Universe;

namespace LudensClub.GeoChaos.Editor.Monitoring.World
{
  public interface IEcsWorldPresenterFactory
  {
    IEcsWorldPresenter Create(IEcsWorldWrapper world, IEcsUniversePresenter parent);
  }
}