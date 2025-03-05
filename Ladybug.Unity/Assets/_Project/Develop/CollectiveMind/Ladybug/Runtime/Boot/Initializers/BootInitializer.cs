using CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class BootInitializer : IInitializable
  {
    private readonly ISceneLoader _sceneLoader;

    public BootInitializer(ISceneLoader sceneLoader)
    {
      _sceneLoader = sceneLoader;
    }
    
    public void Initialize()
    {
      _sceneLoader.LoadAsync(SceneType.Game);
    }
  }
}