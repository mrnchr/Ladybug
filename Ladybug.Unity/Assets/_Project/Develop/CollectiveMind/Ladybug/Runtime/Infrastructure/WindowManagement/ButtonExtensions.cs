using UnityEngine.Events;
using UnityEngine.UI;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public static class ButtonExtensions
  {
    public static void AddListener(this Button button, UnityAction callback)
    {
      if (!button)
        return;
      
      button.onClick.AddListener(callback);
    }
    
    public static void RemoveListener(this Button button, UnityAction callback)
    {
      if (!button)
        return;
      
      button.onClick.RemoveListener(callback);
    }
  }
}