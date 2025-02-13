using UnityEngine.InputSystem;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Input
{
  public class InputInstaller : Installer<PlayerInput, InputInstaller>
  {
    private readonly PlayerInput _input;

    public InputInstaller(PlayerInput input)
    {
      _input = input;
    }
    
    public override void InstallBindings()
    {
      BindLadybugInputActions();
      BindPlayerInput();
      BindInputData();
      BindInputController();
    }

    private void BindLadybugInputActions()
    {
      Container
        .BindInterfacesAndSelfTo<LadybugInputActions>()
        .AsSingle();
    }

    private void BindPlayerInput()
    {
      Container
        .BindInstance(_input)
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