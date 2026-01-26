using System.Threading;
using CollectiveMind.Ladybug.Runtime.Infrastructure;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class ObstacleSpawner
  {
    private readonly ObstacleSpawnConfig _config;
    private readonly ObstacleSpawnService _obstacleSpawnSvc;
    private CancellationTokenSource _cts;

    public ObstacleSpawner(ObstacleSpawnConfig config, ObstacleSpawnService obstacleSpawnSvc)
    {
      _config = config;
      _obstacleSpawnSvc = obstacleSpawnSvc;
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
        _obstacleSpawnSvc.SpawnObstacle();
        
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