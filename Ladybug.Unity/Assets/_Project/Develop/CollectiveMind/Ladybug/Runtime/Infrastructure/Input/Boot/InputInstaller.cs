using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Input
{
  public class InputInstaller : Installer<InputInstaller>
  {
    public override void InstallBindings()
    {
      BindLadybugInputActions();
      BindInputData();
      BindInputController();
    }

    private void BindLadybugInputActions()
    {
      Container
        .BindInterfacesAndSelfTo<LadybugInputActions>()
        .AsSingle();
    }

    private void BindInputData()
    {
      Container
        .Bind<InputData>()
        .AsSingle();
    }

    private void BindInputController()
    {
      Container
        .BindInterfacesTo<InputController>()
        .AsSingle();
    }
  }
}