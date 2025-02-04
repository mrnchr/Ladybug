namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle
{
  public interface ILifeCycleBinder
  {
    void Bind(object obj, LifeCycleFlags flags = LifeCycleFlags.All);
    void Unbind(object obj, LifeCycleFlags flags = LifeCycleFlags.All);
  }
}