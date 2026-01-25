using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Input;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.CoroutineRunner;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.SceneTransition.Boot;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class ProjectInstaller : MonoInstaller
  {
    [SerializeField]
    [InlineProperty]
    [HideLabel]
    private ConfigBinder _configBinder;

    [SerializeField]
    private EventSystem _eventSystem;

    public override void InstallBindings()
    {
      _configBinder.BindConfigs(Container);

      BindCoroutineRunner();
      BindLifeCycleBinder();
      BindFacadePool();
      
      InstallSceneTransition();

      BindEventSystem();
      
      InstallInput();

#if UNITY_EDITOR
      EditorBridge.InstallProject(Container);
#endif
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

    private void InstallSceneTransition()
    {
      SceneTransitionInstaller.Install(Container);
    }

    private void BindEventSystem()
    {
      Container
        .BindInstance(_eventSystem)
        .AsSingle();
    }

    private void InstallInput()
    {
      InputInstaller.Install(Container);
    }
  }
}