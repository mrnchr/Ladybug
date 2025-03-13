using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Input
{
  public class InputController : ITickable
  {
    private readonly LadybugInputActions _actions;
    private readonly InputData _inputData;
    private readonly InputAction _drawAction;
    private readonly InputAction _positionAction;

    public InputController(LadybugInputActions actions, InputData inputData, PlayerInput input)
    {
      _actions = actions;
      _inputData = inputData;

      input.actions = _actions.asset;
      _drawAction = _actions.Gameplay.Draw;
      _positionAction = _actions.Gameplay.Position;
      
      _actions.Gameplay.Enable();
    }

    public void Tick()
    {
      _inputData.Clear();
      
      _inputData.StartDraw = _drawAction.WasPerformedThisFrame();
      _inputData.EndDraw = _drawAction.WasReleasedThisFrame();
      _inputData.Position = _positionAction.ReadValue<Vector2>();
    }
  }
}