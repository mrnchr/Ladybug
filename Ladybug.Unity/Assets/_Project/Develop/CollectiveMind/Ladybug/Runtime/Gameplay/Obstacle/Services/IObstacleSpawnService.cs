using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Obstacle
{
  public interface IObstacleSpawnService
  {
    Vector3 CalculateSpawnPosition();
    bool IsObstacleNear(Vector3 checkedPosition);
    void CreateObstacle(Vector3 spawnPosition);
  }
}