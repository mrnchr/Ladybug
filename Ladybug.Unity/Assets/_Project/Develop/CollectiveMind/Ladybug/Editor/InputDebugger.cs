using CollectiveMind.Ladybug.Runtime.Infrastructure.Input;
using TriInspector;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Editor
{
  public class InputDebugger : MonoBehaviour
  {
    [field: SerializeField]
    [InlineProperty]
    [HideLabel]
    public InputData Data { get; private set; }

    [Inject]
    public void Construct(InputData data)
    {
      Data = data;
    }
  }
}