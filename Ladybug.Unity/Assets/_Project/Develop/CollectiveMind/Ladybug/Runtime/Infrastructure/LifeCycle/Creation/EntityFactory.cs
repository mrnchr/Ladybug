using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using Zenject;
using Object = UnityEngine.Object;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public class EntityFactory
  {
    private readonly Dictionary<EntityType, CreationRecipe> _recipes = new Dictionary<EntityType, CreationRecipe>();
    private readonly IInstantiator _instantiator;
    private readonly IEcsUniverse _universe;
    private readonly GameplayUpdater _gameplayUpdater;
    private readonly CreationContext _cachedContext = new CreationContext();
    private readonly FacadeEcsConverter _facadeEcsConverter;

    public EntityFactory(IInstantiator instantiator, IEcsUniverse universe, GameplayUpdater gameplayUpdater)
    {
      _instantiator = instantiator;
      _universe = universe;
      _gameplayUpdater = gameplayUpdater;
      _facadeEcsConverter = new FacadeEcsConverter();
    }

    public void RegisterRecipe(CreationRecipe creationRecipe)
    {
      _recipes[creationRecipe.EntityType] = creationRecipe.Clone();
    }

    public void UnregisterRecipe(EntityType entityType)
    {
      _recipes.Remove(entityType);
    }

    public EcsEntityWrapper CreateEntity(EntityType entityType)
    {
      var createdEntity = new EcsEntityWrapper();
      _cachedContext.Clear();
      _cachedContext.EntityType = entityType;
      _cachedContext.Entity = createdEntity;
      CreateEntity(_cachedContext);
      return createdEntity;
    }

    public void CreateEntity(EntityType entityType, EcsEntityWrapper entity)
    {
      _cachedContext.Clear();
      _cachedContext.EntityType = entityType;
      _cachedContext.Entity = entity;
      CreateEntity(_cachedContext);
    }

    public EntityVisual CreateVisual(EntityType entityType)
    {
      var createdEntity = new EcsEntityWrapper();
      _cachedContext.Clear();
      _cachedContext.EntityType = entityType;
      _cachedContext.Entity = createdEntity;
      CreateEntity(_cachedContext);
      return _cachedContext.Visual;
    }

    public void CreateEntityWithVisual(EntityType entityType, EntityVisual existingVisual, EcsEntityWrapper cachedEntity)
    {
      _cachedContext.Clear();
      _cachedContext.EntityType = entityType;
      _cachedContext.Visual = existingVisual;
      _cachedContext.Entity = cachedEntity;
      CreateEntity(_cachedContext);
    }

    public EcsEntityWrapper CreateEntityWithVisual(EntityType entityType, EntityVisual existingVisual)
    {
      var createdEntity = new EcsEntityWrapper();
      _cachedContext.Clear();
      _cachedContext.EntityType = entityType;
      _cachedContext.Visual = existingVisual;
      _cachedContext.Entity = createdEntity;
      CreateEntity(_cachedContext);
      return createdEntity;
    }

    public void DestroyEntity(EcsEntityWrapper entity)
    {
      if (entity == null || !entity.IsAlive() || entity.Has<VisualDestroying>())
      {
        return;
      }

      EntityVisual visual = null;
      bool hasVisual = false;

      if (entity.Has<EntityVisualRef>())
      {
        visual = entity.Get<EntityVisualRef>().Visual;
        entity.Add<VisualDestroying>();
        hasVisual = true;
      }

      IFacade facade = null;

      if (entity.Has<FacadeRef>())
      {
        facade = entity.Get<FacadeRef>().Facade;
      }

      (visual as IUnbindable)?.Unbind();
      (facade as IUnbindable)?.Unbind();

      _facadeEcsConverter.DetachFacade(entity);

      if (hasVisual)
      {
        visual.DetachEntity();
      }

      DisposeFacade(facade);

      if (hasVisual)
      {
        visual.OnPreDestroy();
        Object.Destroy(visual.gameObject);
      }

      entity.DelEntity();
    }

    private void CreateEntity(CreationContext context)
    {
      EcsEntityWrapper entity = context.Entity;
      entity.SetWorld(null);

      CreationRecipe recipe;

      if (context.CreationRecipe != null)
      {
        recipe = context.CreationRecipe;
      }
      else if (!_recipes.TryGetValue(context.EntityType, out recipe))
      {
        return;
      }

      int ecsEntityId = _universe.Game.NewEntity();
      entity.SetWorld(_universe.Game, ecsEntityId);

      if (!context.Visual && recipe.VisualPrefab != null)
      {
        context.Visual = _instantiator.InstantiatePrefabForComponent<EntityVisual>(recipe.VisualPrefab);
      }

      if (context.Facade == null && recipe.FacadeType != null)
      {
        context.Facade = (IFacade)_instantiator.Instantiate(recipe.FacadeType);
      }

      recipe.EcsConverter?.ConvertTo(entity);

      if (context.Visual)
      {
        context.Visual.AttachEntity(entity);
      }

      _facadeEcsConverter.AttachFacade(context.Facade, entity);
      
      if (context.Facade is IGameCycle gameCycle)
      {
        _gameplayUpdater.Add(gameCycle);
      }

      (context.Facade as IBindable)?.Bind(entity);
      (context.Visual as IBindable)?.Bind(entity);
    }

    private void DisposeFacade(IFacade facade)
    {
      if (facade is IGameCycle gameCycle)
      {
        _gameplayUpdater.Remove(gameCycle);
      }

      (facade as IDisposable)?.Dispose();
    }
  }
}