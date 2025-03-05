using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.Boot
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