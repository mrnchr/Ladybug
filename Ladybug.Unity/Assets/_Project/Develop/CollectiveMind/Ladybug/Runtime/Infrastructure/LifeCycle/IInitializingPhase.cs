using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle
{
  public interface IInitializingPhase
  {
    bool WasInitialized { get; }
    void EnsureInitializing(IInitializable initializable);
    bool Add(IInitializable initializable);
  }
}