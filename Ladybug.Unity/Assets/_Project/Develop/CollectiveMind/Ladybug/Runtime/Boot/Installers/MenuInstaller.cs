using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement.Boot;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class MenuInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      InstallWindow();
    }

    private void InstallWindow()
    {
      WindowInstaller.Install(Container);
    }
  }
}