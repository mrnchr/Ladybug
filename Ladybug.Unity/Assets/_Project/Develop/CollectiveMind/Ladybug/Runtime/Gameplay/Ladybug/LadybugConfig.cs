using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  [CreateAssetMenu(menuName = CAC.Names.LADYBUG_CONFIG_MENU, fileName = CAC.Names.LADYBUG_CONFIG_FILE)]
  public class LadybugConfig : ScriptableObject
  {
    [Title("Movement")]
    public float Speed;

    public float AnimationSpeedMultiplier = 1;
    public float ViewDistance;
    public float ViewWidth;
    
    [Title("Invincibility")]
    public float InvincibilityTime;
    public float MinInvincibleAlpha;
    public int BlinkCount = 1;
    public List<MaterialPair> InterchangeableMaterials;
  }

  [Serializable]
  [DeclareHorizontalGroup(nameof(MaterialPair))]
  public struct MaterialPair
  {
    [GroupNext(nameof(MaterialPair))]
    [HideLabel]
    public Material Opaque;
    [HideLabel]
    public Material Transparent;
  }
}