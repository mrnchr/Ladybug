using CollectiveMind.Ladybug.Editor.Monitoring.Component;
using CollectiveMind.Ladybug.Editor.Monitoring.Entity;
using CollectiveMind.Ladybug.Editor.Monitoring.Sorting;
using CollectiveMind.Ladybug.Editor.Monitoring.Universe;
using CollectiveMind.Ladybug.Editor.Monitoring.World;
using Zenject;

namespace CollectiveMind.Ladybug.Editor.Boot.Installers
{
  public class EditorLevelInstaller : Installer<EditorLevelInstaller>
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