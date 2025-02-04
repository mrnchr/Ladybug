using LudensClub.GeoChaos.Editor.Monitoring.Component;
using LudensClub.GeoChaos.Editor.Monitoring.Entity;
using LudensClub.GeoChaos.Editor.Monitoring.Sorting;
using LudensClub.GeoChaos.Editor.Monitoring.Universe;
using LudensClub.GeoChaos.Editor.Monitoring.World;
using Zenject;

namespace CollectiveMind.Ladybug.Editor.Boot.Installers
{
  public class DebugLevelInstaller : Installer<DebugLevelInstaller>
  {
    public override void InstallBindings()
    {
      BindEcsComponentSorter();
      BindEcsComponentViewFactory();

      BindEcsUniverseViewFactory();
      BindEcsWorldPresenterFactory();
      BindEcsWorldViewFactory();
      BindEcsEntityPresenterFactory();
      BindEcsEntityViewFactory();
      BindEcsUniversePresenter();
    }

    private void BindEcsComponentSorter()
    {
      Container
        .Bind<IEcsComponentSorter>()
        .To<EcsComponentSorter>()
        .AsSingle();
    }

    private void BindEcsComponentViewFactory()
    {
      Container
        .Bind<IEcsComponentViewFactory>()
        .To<EcsComponentViewFactory>()
        .AsSingle();
    }

    private void BindEcsUniverseViewFactory()
    {
      Container
        .Bind<IEcsUniverseViewFactory>()
        .To<EcsUniverseViewFactory>()
        .AsSingle();
    }

    private void BindEcsWorldPresenterFactory()
    {
      Container
        .Bind<IEcsWorldPresenterFactory>()
        .To<EcsWorldPresenterFactory>()
        .AsSingle();
    }

    private void BindEcsWorldViewFactory()
    {
      Container
        .Bind<IEcsWorldViewFactory>()
        .To<EcsWorldViewFactory>()
        .AsSingle();
    }

    private void BindEcsEntityPresenterFactory()
    {
      Container
        .Bind<IEcsEntityPresenterFactory>()
        .To<EcsEntityPresenterFactory>()
        .AsSingle();
    }

    private void BindEcsEntityViewFactory()
    {
      Container
        .Bind<IEcsEntityViewFactory>()
        .To<EcsEntityViewFactory>()
        .AsSingle();
    }

    private void BindEcsUniversePresenter()
    {
      Container
        .BindInterfacesTo<EcsUniversePresenter>()
        .AsSingle();
    }
  }
}