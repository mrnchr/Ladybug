using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using LudensClub.GeoChaos.Editor.Monitoring.World;

namespace LudensClub.GeoChaos.Editor.Monitoring.Entity
{
  public interface IEcsEntityPresenterFactory
  {
    IEcsEntityPresenter Create(int entity, IEcsWorldPresenter parent, IEcsWorldWrapper wrapper);
  }
}