using CollectiveMind.Ladybug.Runtime.Boot.Initializers;
using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Input;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.CoroutineRunner;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Timing;
using CollectiveMind.Ladybug.Runtime.SceneTransition;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Boot
{
  public class ProjectInstaller : MonoInstaller
  {
    [SerializeField]
    private ConfigProvider _configProvider;

    [SerializeField]
    private EventSystem _eventSystem;

    public override void InstallBindings()
    {
      foreach (ScriptableObject config in _configProvider.Configs)
      {
        if (config)
        {
          Container
            .Bind(config.GetType())
            .FromInstance(config)
            .AsSingle();
        }
      }      

      BindCoroutineRunner();
      BindLifeCycleBinder();

      Container
        .Bind<TimeService>()
        .AsSingle();
      Container
        .Bind<TimerFactory>()
        .AsSingle();
      
      InstallSceneTransition();

      BindEventSystem();
      
      InstallInput();

      Container
        .BindInterfacesTo<ProjectInitializer>()
        .AsSingle();

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