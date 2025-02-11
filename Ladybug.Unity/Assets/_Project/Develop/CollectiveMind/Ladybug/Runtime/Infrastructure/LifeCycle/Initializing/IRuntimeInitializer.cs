using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle
{
  public interface IRuntimeInitializer
  {
    bool WasInitialized { get; }
    void Add(IInitializable initializable);
  }
}