using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public class WindowInstaller : Installer<WindowInstaller>
  {
    public override void InstallBindings()
    {
      BindWindowManager();
      BindWindowInitializer();
    }

    private void BindWindowManager()
    {
      Container
        .Bind<IWindowManager>()
        .To<WindowManager>()
        .AsSingle();
    }

    private void BindWindowInitializer()
    {
      Container
        .Bind<WindowInitializer>()
        .AsSingle();
    }
  }
}