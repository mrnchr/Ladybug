namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsEntitySelector
  {
    void Select<TComponent>(EcsEntities origins, EcsEntities targets, EcsEntities selections) where TComponent : struct, IEcsComponent;
  }
}