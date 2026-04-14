using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.PingPongBall
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + "PingPongBall", fileName = nameof(PingPongBallConfig))]
  public class PingPongBallConfig : ScriptableObject
  {
    [LabelText("Ping Pong Ball Speed")]
    public float Speed;

    public bool UseAcceleration;
  }
}