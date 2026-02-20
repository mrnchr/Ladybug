using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using R3;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class CoreEntityInitializerRegistrar : IDisposable
  {
    private readonly EntityFactory _entityFactory;
    private DisposableBag _disposables;

    public CoreEntityInitializerRegistrar(EntityFactory entityFactory)
    {
      _entityFactory = entityFactory;
    }

    public void RegisterInitializers()
    {
      RegisterInitializer<ObstacleTransformInitializer>();
      RegisterInitializer<ObstacleYawDeviationInitializer>();
    }

    public void Dispose()
    {
      _disposables.Dispose();
    }

    private void RegisterInitializer<T>() where T : IEntityInitializer
    {
      _disposables.Add(_entityFactory.RegisterInitializer<T>());
    }
  }
}