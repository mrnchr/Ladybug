using R3;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading
{
  public class SceneLoadingData
  {
    public ReactiveProperty<float> Progress { get; } = new ReactiveProperty<float>();
  }
}