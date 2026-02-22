using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using TriInspector;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [Serializable]
  [EcsComponentOrder(EcsComponentOrder.STATIC)]
  public struct AngularDeviation : IEcsComponent
  {
#if UNITY_EDITOR
    public const string ANGULAR_DEVIATION = "Angular Deviation";
    
    [LabelText(ANGULAR_DEVIATION)]
#endif
    public float MaxAngle;

  }
}