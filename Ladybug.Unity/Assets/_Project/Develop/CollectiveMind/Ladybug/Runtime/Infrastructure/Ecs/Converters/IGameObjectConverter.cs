using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IGameObjectConverter : IEcsConverter
  {
    bool ShouldCreateEntity { get; set; }
    bool CreateEntityOnStart { get; set; }
    bool CreateEntityOnEnable { get; set; }
    void CreateEntity();
    void CreateEntity(EcsEntity entity);
    void ConvertBackAndDestroy(EcsEntity entity);
    void SetEntity(EcsEntity entity);
    void SetEntity(EcsWorld world, int entity);
  }
}