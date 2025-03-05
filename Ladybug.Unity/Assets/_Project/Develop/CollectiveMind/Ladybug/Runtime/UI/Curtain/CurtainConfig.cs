using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.UI
{
  [CreateAssetMenu(menuName = CAC.Names.CURTAIN_CONFIG_MENU, fileName = CAC.Names.CURTAIN_CONFIG_FILE)]
  public class CurtainConfig : ScriptableObject
  {
    public float ShowTime;
  }
}