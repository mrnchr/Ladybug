using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  [CreateAssetMenu(menuName = CAC.Names.LADYBUG_CONFIG_MENU, fileName = CAC.Names.LADYBUG_CONFIG_FILE)]
  public class LadybugConfig : ScriptableObject
  {
    public float Speed;
    public float ViewDistance;
  }
}