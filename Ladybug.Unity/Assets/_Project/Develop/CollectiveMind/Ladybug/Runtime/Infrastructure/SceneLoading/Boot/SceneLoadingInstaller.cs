using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading
{
  public class SceneLoadingInstaller : Installer<SceneLoadingInstaller>
  {
    public override void InstallBindings()
    {
      BindSceneLoadingData();
      BindSceneLoader();
    }

    private void BindSceneLoadingData()
    {
      Container
        .Bind<SceneLoadingData>()
        .AsSingle();
    }

    private void BindSceneLoader()
    {
      Container
        .BindInterfacesTo<SceneLoader>()
        .AsSingle();
    }
  }
}