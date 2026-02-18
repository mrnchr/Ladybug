using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public interface IBindable
  {
    void Bind(EcsEntityWrapper entity);
  }
}