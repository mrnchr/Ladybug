using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  public class ObstacleInstaller : Installer<ObstacleInstaller>
  {
    public override void InstallBindings()
    {
      BindObstacleSpawnService();
      BindObstacleSpawner();
    }

    private void BindObstacleSpawnService()
    {
      Container
        .Bind<ObstacleSpawnService>()
        .AsSingle();
    }

    private void BindObstacleSpawner()
    {
      Container
        .Bind<ObstacleSpawner>()
        .AsSingle();
    }
  }
}