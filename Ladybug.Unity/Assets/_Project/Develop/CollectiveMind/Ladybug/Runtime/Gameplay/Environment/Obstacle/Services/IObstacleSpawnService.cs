using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public interface IObstacleSpawnService
  {
    Vector3 CalculateSpawnPosition();
    bool IsObstacleNear(Vector2 checkedPosition);
    void CreateObstacle(Vector3 spawnPosition);
  }
}