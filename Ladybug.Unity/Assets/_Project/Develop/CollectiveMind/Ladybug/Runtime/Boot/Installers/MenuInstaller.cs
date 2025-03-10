using CollectiveMind.Ladybug.Runtime.Boot.Initializers;
using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement.Boot;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class MenuInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      InstallWindow();

      BindMenuInitializer();
    }

    private void BindMenuInitializer()
    {
      Container
        .BindInterfacesTo<MenuInitializer>()
        .AsSingle();
    }

    private void InstallWindow()
    {
      WindowInstaller.Install(Container);
    }
  }
}