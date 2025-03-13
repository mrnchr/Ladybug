using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.HUD
{
  public class HUDInstaller : Installer<HUDInstaller>
  {
    public override void InstallBindings()
    {
      BindHealthBarFacade();
      BindScoreFacade();
    }

    private void BindHealthBarFacade()
    {
      Container
        .BindInterfacesAndSelfTo<HealthBarFacade>()
        .AsSingle();
    }

    private void BindScoreFacade()
    {
      Container
        .BindInterfacesAndSelfTo<ScoreFacade>()
        .AsSingle();
    }
  }
}