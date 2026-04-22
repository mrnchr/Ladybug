using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.UI.HUD
{
  public class HUDWindow : BaseWindow
  {
    [SerializeField]
    private Transform _signalParent;

    public void SetSignalParent(Transform signal)
    {
      signal.SetParent(_signalParent);
    }
  }
}