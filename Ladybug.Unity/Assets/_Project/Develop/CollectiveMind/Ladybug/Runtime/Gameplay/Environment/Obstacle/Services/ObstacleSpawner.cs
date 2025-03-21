﻿using System;
using CollectiveMind.Ladybug.Runtime.Configuration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class ObstacleSpawner : IInitializable, IDisposable
  {
    private readonly IObstacleSpawnService _obstacleSpawnSvc;
    private bool _isSpawning;
    private readonly ObstacleSpawnConfig _config;

    public ObstacleSpawner(IConfigProvider configProvider, IObstacleSpawnService obstacleSpawnSvc)
    {
      _obstacleSpawnSvc = obstacleSpawnSvc;
      _config = configProvider.Get<ObstacleSpawnConfig>();
    }

    public void Initialize()
    {
      SpawnObstacle();
    }

    private async void SpawnObstacle()
    {
      _isSpawning = true;
      await UniTask.WaitForSeconds(Random.Range(_config.SpawnTime.x, _config.SpawnTime.y));

      while (_isSpawning)
      {
        Vector3 rawSpawnPosition = _obstacleSpawnSvc.CalculateSpawnPosition();
        var spawnPosition = new Vector2(rawSpawnPosition.x, rawSpawnPosition.z);
        if (!_obstacleSpawnSvc.IsObstacleNear(spawnPosition))
          _obstacleSpawnSvc.CreateObstacle(rawSpawnPosition);

        await UniTask.WaitForSeconds(Random.Range(_config.SpawnTime.x, _config.SpawnTime.y));
      }
    }

    public void Dispose()
    {
      _isSpawning = false;
    }
  }
}