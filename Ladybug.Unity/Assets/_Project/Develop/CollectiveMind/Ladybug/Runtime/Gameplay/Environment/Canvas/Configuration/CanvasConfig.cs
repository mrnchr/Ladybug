using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas
{
  [CreateAssetMenu(menuName = CAC.Names.CANVAS_CONFIG_MENU, fileName = CAC.Names.CANVAS_CONFIG_FILE)]
  public class CanvasConfig : ScriptableObject
  {
    public int CanvasScale;
    
    public float CanvasSize => CanvasScale * 10;
  }
}