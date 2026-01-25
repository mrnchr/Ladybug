using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  [CreateAssetMenu(menuName = CAC.Names.CAMERA_CONFIG_MENU, fileName = CAC.Names.CAMERA_CONFIG_FILE)]
  public class CameraConfig : ScriptableObject
  {
    public List<CameraKeyframe> Positions;
    public float ShakingDuration;
    public Color ShakingColor;
  }

  [Serializable]
  [DeclareHorizontalGroup(nameof(CameraKeyframe))]
  public class CameraKeyframe
  {
    [GroupNext(nameof(CameraKeyframe))]
    [HideLabel]
    public DirectionType Direction;
    
    [HideLabel]
    public float Offset;
  }
}