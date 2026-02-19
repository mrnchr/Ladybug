using System;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class CoreCreationRecipeRegistrar : IDisposable
  {
    private readonly EntityFactory _entityFactory;
    private readonly EcsConvertersProvider _converters;
    private readonly PrefabsProvider _prefabs;
    private readonly CreationRecipe _reusableRecipe;
    private DisposableBag _disposables;

    public CoreCreationRecipeRegistrar(EntityFactory entityFactory, EcsConvertersProvider converters, PrefabsProvider prefabs)
    {
      _entityFactory = entityFactory;
      _converters = converters;
      _prefabs = prefabs;
      _reusableRecipe = new CreationRecipe();
    }

    public void RegisterRecipes()
    {
      RegisterRecipe<CameraFacade>(EntityType.Camera);
      RegisterRecipe(EntityType.VirtualCamera);
      RegisterRecipe<CameraTargetFacade>(EntityType.CameraTarget);
      RegisterRecipe<SpawnPointFacade>(EntityType.SpawnPoint);
      RegisterRecipe<LadybugFacade>(EntityType.Ladybug);
      RegisterRecipe(EntityType.Canvas);

      foreach (EntityType obstacle in EntityTypeUtils.Obstacles)
      {
        RegisterRecipe(obstacle);
      }
    }

    public void Dispose()
    {
      _disposables.Dispose();
    }

    private void RegisterRecipe(EntityType entityType)
    {
      ConfigureRecipe(entityType);
      _disposables.Add(_entityFactory.RegisterRecipe(_reusableRecipe));
    }

    private void RegisterRecipe<TFacade>(EntityType entityType) where TFacade : IFacade
    {
      ConfigureRecipe<TFacade>(entityType);
      _disposables.Add(_entityFactory.RegisterRecipe(_reusableRecipe));
    }

    private void ConfigureRecipe(EntityType entityType)
    {
      _reusableRecipe.Replace(entityType, _converters.Get(entityType), _prefabs.Get(entityType), null);
    }

    private void ConfigureRecipe<TFacade>(EntityType entityType) where TFacade : IFacade
    {
      ConfigureRecipe(entityType);
      _reusableRecipe.FacadeType = typeof(TFacade);
    }
  }
}