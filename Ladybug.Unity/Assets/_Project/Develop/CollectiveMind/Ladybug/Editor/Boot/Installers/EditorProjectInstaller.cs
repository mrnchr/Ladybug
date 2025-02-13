using Zenject;

namespace CollectiveMind.Ladybug.Editor.Boot.Installers
{
  public class EditorProjectInstaller : Installer<EditorProjectInstaller>
  {
    public override void InstallBindings()
    {
      BindInputDebugger();
    }
    
    private void BindInputDebugger()
    {
      Container
        .Bind<InputDebugger>()
        .FromNewComponentOnNewGameObject()
        .WithGameObjectName($"[{nameof(InputDebugger)}]")
        .AsSingle()
        .NonLazy();
    }
  }
}