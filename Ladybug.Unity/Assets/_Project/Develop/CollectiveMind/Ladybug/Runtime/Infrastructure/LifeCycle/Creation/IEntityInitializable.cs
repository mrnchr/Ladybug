namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public interface IEntityInitializable
  {
    void Initialize(EntityInitContext initContext);
  }
}