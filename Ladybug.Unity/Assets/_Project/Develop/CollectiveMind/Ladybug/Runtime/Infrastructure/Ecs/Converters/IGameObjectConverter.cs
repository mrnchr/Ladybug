using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IGameObjectConverter : IEcsConverter
  {
    bool ShouldCreateEntity { get; set; }
    bool CreateEntityOnStart { get; set; }
    void CreateEntity();
    void CreateEntity(EcsEntityWrapper entity);
    void ConvertBackAndDestroy(EcsEntityWrapper entity);
    void SetEntity(EcsEntityWrapper entity);
    void SetEntity(EcsWorld world, int entity);
  }
}