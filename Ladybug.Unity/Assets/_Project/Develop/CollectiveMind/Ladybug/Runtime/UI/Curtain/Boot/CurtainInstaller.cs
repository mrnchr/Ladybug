using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.Curtain
{
  public class CurtainInstaller : Installer<CurtainInstaller>
  {
    public override void InstallBindings()
    {
      BindCurtainFacade();
    }

    private void BindCurtainFacade()
    {
      Container
        .Bind<CurtainFacade>()
        .AsSingle();
    }
  }
}