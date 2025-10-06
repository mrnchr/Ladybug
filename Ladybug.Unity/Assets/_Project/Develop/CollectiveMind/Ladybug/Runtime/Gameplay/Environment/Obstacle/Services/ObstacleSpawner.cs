using System.Threading;
using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class ObstacleSpawner
  {
    private readonly IObstacleSpawnService _obstacleSpawnSvc;
    private readonly ObstacleSpawnConfig _config;
    private CancellationTokenSource _cts;

    public ObstacleSpawner(IConfigProvider configProvider, IObstacleSpawnService obstacleSpawnSvc)
    {
      _obstacleSpawnSvc = obstacleSpawnSvc;
      _config = configProvider.Get<ObstacleSpawnConfig>();
    }

    public void StartSpawn()
    {
      _cts = new CancellationTokenSource();
      SpawnObstacle(_cts.Token);
    }

    private async void SpawnObstacle(CancellationToken token = default(CancellationToken))
    {
      await UniTask.WaitForSeconds(Random.Range(_config.SpawnTime.x, _config.SpawnTime.y), cancellationToken: token)
        .SuppressCancellationThrow();

      while (!token.IsCancellationRequested)
      {
        Vector3 rawSpawnPosition = _obstacleSpawnSvc.CalculateSpawnPosition();
        var spawnPosition = new Vector2(rawSpawnPosition.x, rawSpawnPosition.z);
        if (!_obstacleSpawnSvc.IsObstacleNear(spawnPosition))
          _obstacleSpawnSvc.CreateObstacle(rawSpawnPosition);

        await UniTask.WaitForSeconds(Random.Range(_config.SpawnTime.x, _config.SpawnTime.y), cancellationToken: token)
          .SuppressCancellationThrow();
      }
    }

    public void StopSpawn()
    {
      _cts = _cts?.CancelDisposeAndForget();
    }
  }
}