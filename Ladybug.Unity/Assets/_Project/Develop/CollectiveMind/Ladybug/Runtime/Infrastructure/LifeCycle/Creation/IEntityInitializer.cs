namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public interface IEntityInitializer
  {
    void InitializeEntity(CreationContext creationContext);
  }
}