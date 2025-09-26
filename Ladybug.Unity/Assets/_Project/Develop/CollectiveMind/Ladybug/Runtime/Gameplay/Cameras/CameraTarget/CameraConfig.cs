using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget
{
  [CreateAssetMenu(menuName = CAC.Names.CAMERA_CONFIG_MENU, fileName = CAC.Names.CAMERA_CONFIG_FILE)]
  public class CameraConfig : ScriptableObject
  {
    public float Speed;
    public float SmoothTime;
    public float MinSmoothStep;
    
    [LabelText("Speed Rates By Ladybug Position On The Screen")]
    public List<float> SpeedRates = new List<float>();
  }
}