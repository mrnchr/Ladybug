using System.Collections.Generic;
using System.Linq;
using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Obstacle
{
  public class ObstacleSpawnService : IObstacleSpawnService
  {
    private readonly IEcsUniverse _universe;
    private readonly IViewFactory _viewFactory;
    private readonly EcsEntities _ladybugs;
    private readonly ObstacleSpawnConfig _config;
    private readonly EcsEntities _obstacles;

    public ObstacleSpawnService(IEcsUniverse universe, IConfigProvider configProvider, IViewFactory viewFactory)
    {
      _universe = universe;
      _viewFactory = viewFactory;
      _config = configProvider.Get<ObstacleSpawnConfig>();

      _ladybugs = _universe
        .FilterGame<LadybugTag>()
        .Inc<ConverterRef>()
        .Collect();

      _obstacles = _universe
        .FilterGame<ObstacleTag>()
        .Inc<ConverterRef>()
        .Collect();
    }
    
    public Vector3 CalculateSpawnPosition()
    {
      foreach (EcsEntityWrapper ladybug in _ladybugs)
      {
        Transform transform = ladybug.Get<TransformRef>().Transform;
        return transform.position + transform.forward * Random.Range(_config.SpawnDistance.x, _config.SpawnDistance.y);
      }

      return Vector3.negativeInfinity;
    }

    public bool IsObstacleNear(Vector2 checkedPosition)
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

    public void CreateObstacle(Vector3 spawnPosition)
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