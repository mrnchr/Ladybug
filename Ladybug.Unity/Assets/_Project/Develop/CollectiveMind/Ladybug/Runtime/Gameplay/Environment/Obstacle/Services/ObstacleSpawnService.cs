using System.Collections.Generic;
using System.Linq;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class ObstacleSpawnService
  {
    private readonly ObstacleSpawnConfig _config;
    private readonly IViewFactory _viewFactory;
    private readonly EcsEntities _cameras;
    private readonly EcsEntities _ladybugs;
    private readonly EcsEntities _obstacles;

    public ObstacleSpawnService(IEcsUniverse universe, ObstacleSpawnConfig config, IViewFactory viewFactory)
    {
      _viewFactory = viewFactory;
      _config = config;

      _cameras = universe
        .FilterGame<CameraTag>()
        .Inc<CameraData>()
        .Collect();

      _ladybugs = universe
        .FilterGame<LadybugTag>()
        .Inc<ConverterRef>()
        .Collect();

      _obstacles = universe
        .FilterGame<ObstacleTag>()
        .Inc<ConverterRef>()
        .Collect();
    }

    public void SpawnObstacle()
    {
      Vector3 rawSpawnPosition = CalculateSpawnPosition();
      var spawnPosition = new Vector2(rawSpawnPosition.x, rawSpawnPosition.z);
      if (!IsObstacleNear(spawnPosition))
        CreateObstacle(rawSpawnPosition);
    }

    private Vector3 CalculateSpawnPosition()
    {
      foreach (EcsEntityWrapper camera in _cameras)
      {
        Rect worldBounds = camera.Get<CameraData>().WorldXZBounds;
        float cameraDiagonal = Mathf.Sqrt(Mathf.Pow(worldBounds.width, 2) + Mathf.Pow(worldBounds.height, 2));
        float safetyDiagonal = cameraDiagonal * _config.CameraDiagonalSafetyMultiplier;
        
        foreach (EcsEntityWrapper ladybug in _ladybugs)
        {
          float angularDeviation = Random.Range(-_config.AngularDeviation, _config.AngularDeviation);
          float extraDistance = Random.Range(_config.DistanceRange.x, _config.DistanceRange.y);
          float distance = safetyDiagonal + _config.MaxObstacleSize + extraDistance;
          Transform transform = ladybug.Get<TransformRef>().Transform;
          Vector3 deviatedForward = Quaternion.AngleAxis(angularDeviation, Vector3.up) * transform.forward;
          return transform.position + deviatedForward.normalized * distance;
        }
      }

      return Vector3.negativeInfinity;
    }

    private bool IsObstacleNear(Vector2 checkedPosition)
    {
      foreach (EcsEntityWrapper obstacle in _obstacles)
      {
        Vector3 rawPosition = obstacle.Get<TransformRef>().Transform.position;
        var position = new Vector2(rawPosition.x, rawPosition.z);
        if ((checkedPosition - position).sqrMagnitude < Mathf.Pow(_config.DistanceBetweenObstacles, 2))
          return true;
      }

      return false;
    }

    private void CreateObstacle(Vector3 spawnPosition)
    {
      EntityType entityType = SelectObstacleType();
      var converter = _viewFactory.Create<GameObjectConverter>(entityType);
      Quaternion rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
      converter.transform.SetPositionAndRotation(spawnPosition, rotation);
    }

    private EntityType SelectObstacleType()
    {
      List<float> chances = _config.SpawnChances.Select(x => x.Chance).ToList();
      return _config.SpawnChances[ChooseRandom(chances)].EntityType;
    }
    
    private static int ChooseRandom(List<float> chances)
    {
      float probability = Random.value;
      for (var i = 0; i < chances.Count; i++)
      {
        float chance = chances[i];
        if (probability < chance)
          return i;

        probability -= chance;
      }

      return chances.Count - 1;
    }
  }
}