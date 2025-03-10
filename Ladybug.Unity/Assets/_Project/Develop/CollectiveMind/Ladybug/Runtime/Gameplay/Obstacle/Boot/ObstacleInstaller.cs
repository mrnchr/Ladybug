using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Obstacle
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
        .Bind<IObstacleSpawnService>()
        .To<ObstacleSpawnService>()
        .AsSingle();
    }

    private void BindObstacleSpawner()
    {
      Container
        .BindInterfacesTo<ObstacleSpawner>()
        .AsSingle();
    }
  }
}