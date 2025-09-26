using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Session
{
  [CreateAssetMenu(menuName = CAC.Names.GAME_SESSION_CONFIG_MENU,
    fileName = CAC.Names.GAME_SESSION_CONFIG_FILE)]
  public class GameSessionConfig : ScriptableObject
  {
    public int HealthPoints;
    public int RevivalCount;
    
    public float RaiseRate;
    public float RaiseDistance;
  }
}