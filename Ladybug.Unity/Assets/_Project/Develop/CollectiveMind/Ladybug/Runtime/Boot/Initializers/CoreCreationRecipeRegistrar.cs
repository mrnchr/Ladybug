using System;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.CameraTarget;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;

namespace CollectiveMind.Ladybug.Runtime.Boot.Initializers
{
  public class CoreCreationRecipeRegistrar : IDisposable
  {
    private readonly EntityFactory _entityFactory;
    private readonly EcsConvertersProvider _converters;
    private readonly PrefabsProvider _prefabs;
    private readonly CreationRecipe _reusableRecipe;

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
      _entityFactory.UnregisterRecipe(EntityType.Camera);
      _entityFactory.UnregisterRecipe(EntityType.VirtualCamera);
      _entityFactory.UnregisterRecipe(EntityType.CameraTarget);
      _entityFactory.UnregisterRecipe(EntityType.SpawnPoint);
      _entityFactory.UnregisterRecipe(EntityType.Ladybug);
      _entityFactory.UnregisterRecipe(EntityType.Canvas);
      
      foreach (EntityType obstacle in EntityTypeUtils.Obstacles)
      {
        _entityFactory.UnregisterRecipe(obstacle);
      }
    }

    private void RegisterRecipe(EntityType entityType)
    {
      ReplaceRecipe(entityType);
      _entityFactory.RegisterRecipe(_reusableRecipe);
    }

    private void ReplaceRecipe(EntityType entityType)
    {
      _reusableRecipe.Replace(entityType, _converters.Get(entityType), _prefabs.Get(entityType), null);
    }

    private void RegisterRecipe<TFacade>(EntityType entityType) where TFacade : IFacade
    {
      ReplaceRecipe<TFacade>(entityType);
      _entityFactory.RegisterRecipe(_reusableRecipe);
    }

    private void ReplaceRecipe<TFacade>(EntityType entityType) where TFacade : IFacade
    {
      ReplaceRecipe(entityType);
      _reusableRecipe.FacadeType = typeof(TFacade);
    }
  }
}