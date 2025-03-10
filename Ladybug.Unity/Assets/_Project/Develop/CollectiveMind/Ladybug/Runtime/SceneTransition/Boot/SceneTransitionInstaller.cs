using CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading;
using CollectiveMind.Ladybug.Runtime.UI.Curtain;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.SceneTransition.Boot
{
  public class SceneTransitionInstaller : Installer<SceneTransitionInstaller>
  {
    public override void InstallBindings()
    {
      InstallSceneLoading();
      BindGameSwitcher();
      InstallCurtain();
    }

    private void InstallSceneLoading()
    {
      SceneLoadingInstaller.Install(Container);
    }

    private void BindGameSwitcher()
    {
      Container
        .Bind<IGameSwitcher>()
        .To<GameSwitcher>()
        .AsSingle();
    }

    private void InstallCurtain()
    {
      CurtainInstaller.Install(Container);
    }
  }
}