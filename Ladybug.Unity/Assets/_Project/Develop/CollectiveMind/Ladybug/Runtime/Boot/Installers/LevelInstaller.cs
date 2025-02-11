using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class LevelInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindLineDrawer();
      BindLadybugRotator();
    }

    private void BindLadybugRotator()
    {
      Container
        .Bind<ILadybugRotator>()
        .To<LadybugRotator>()
        .AsSingle();
    }

    private void BindLineDrawer()
    {
      Container
        .BindInterfacesTo<LineDrawer>()
        .AsSingle();
    }
  }
}