using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading
{
  public interface ISceneLoader
  {
    UniTask LoadAsync(SceneType id);
  }
}