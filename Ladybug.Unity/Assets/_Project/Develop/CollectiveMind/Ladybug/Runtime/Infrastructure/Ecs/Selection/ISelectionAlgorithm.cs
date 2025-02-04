namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface ISelectionAlgorithm
  {
    void Select(EcsEntities origins, EcsEntities marks);
  }
}