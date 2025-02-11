using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.CoroutineRunner;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class ProjectInstaller : MonoInstaller
  {
    [SerializeField]
    private ScriptableConfigProvider _configProvider;

    public override void InstallBindings()
    {
      BindConfigProvider();

      BindCoroutineRunner();
      BindLifeCycleBinder();
      BindFacadePool();

#if UNITY_EDITOR
      EditorBridge.InstallProject(Container);
#endif
    }

    private void BindConfigProvider()
    {
      Container
        .Bind<IConfigProvider>()
        .FromInstance(_configProvider)
        .AsSingle();
    }

    private void BindCoroutineRunner()
    {
      Container
        .Bind<ICoroutineRunner>()
        .To<CoroutineRunner>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName("CoroutineRunner")
        .UnderTransform(ctx => ctx.Container.Resolve<Context>().transform)
        .AsSingle()
        .CopyIntoAllSubContainers();
    }

    private void BindLifeCycleBinder()
    {
      Container
        .Bind<ILifeCycleBinder>()
        .To<LifeCycleBinder>()
        .AsSingle()
        .CopyIntoAllSubContainers();
    }

    private void BindFacadePool()
    {
      Container
        .Bind<IFacadePool>()
        .To<FacadePool>()
        .AsSingle()
        .CopyIntoAllSubContainers();
    }
  }
}