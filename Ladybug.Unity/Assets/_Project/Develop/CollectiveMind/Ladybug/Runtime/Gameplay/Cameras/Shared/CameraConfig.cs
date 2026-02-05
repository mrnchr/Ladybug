using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Cameras
{
  [CreateAssetMenu(menuName = CAC.Names.CAMERA_CONFIG_MENU, fileName = CAC.Names.CAMERA_CONFIG_FILE)]
  public class CameraConfig : ScriptableObject
  {
    [Title("Moving")]
    [Range(0, 1)]
    public float CameraSpeedMultiplier = 1;

    [Title("Boost")]
    [Range(0, 1)]
    public float BoostZPosition;

    [LabelText("Damage Boost Time")]
    public float DamageBoostDuration;

    public float FrameOffset;
    
    [Title("Shaking")]
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