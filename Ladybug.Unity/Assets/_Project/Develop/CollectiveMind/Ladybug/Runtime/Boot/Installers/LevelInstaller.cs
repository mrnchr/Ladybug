using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class LevelInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindLineDrawer();
    }

    private void BindLineDrawer()
    {
      Container
        .BindInterfacesTo<LineDrawer>()
        .AsSingle();
    }
  }
}