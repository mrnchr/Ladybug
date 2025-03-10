using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Obstacle
{
  public class ObstacleSpawnService : IObstacleSpawnService
  {
    public Vector3 CalculateSpawnPosition()
    {
      return Vector3.zero;
    }

    public bool IsObstacleNear(Vector3 checkedPosition)
    {
      return true;
    }

    public void CreateObstacle(Vector3 spawnPosition)
    {
    }
  }
}