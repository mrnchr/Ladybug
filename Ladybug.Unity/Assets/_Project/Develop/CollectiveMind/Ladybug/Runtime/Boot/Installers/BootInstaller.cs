using CollectiveMind.Ladybug.Runtime.Boot.Initializers;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class BootInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindBootInitializer();
    }

    private void BindBootInitializer()
    {
      Container
        .BindInterfacesTo<BootInitializer>()
        .AsSingle();
    }
  }
}