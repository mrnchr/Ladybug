using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Input
{
  public class InputController : ITickable
  {
    private readonly LadybugInputActions _actions;
    private readonly InputData _inputData;
    private readonly EventSystem _eventSystem;

    public InputController(LadybugInputActions actions, InputData inputData, EventSystem eventSystem)
    {
      _actions = actions;
      _inputData = inputData;
      _eventSystem = eventSystem;

      _actions.Gameplay.Enable();
      _actions.UI.Enable();
    }

    public void Tick()
    {
      _inputData.Clear();

      _inputData.StartDraw = _actions.Gameplay.Draw.WasPerformedThisFrame() && !_eventSystem.IsPointerOverGameObject();
      _inputData.EndDraw = _actions.Gameplay.Draw.WasReleasedThisFrame() && !_eventSystem.IsPointerOverGameObject();
      _inputData.Position = _actions.Gameplay.Position.ReadValue<Vector2>();
    }
  }
}