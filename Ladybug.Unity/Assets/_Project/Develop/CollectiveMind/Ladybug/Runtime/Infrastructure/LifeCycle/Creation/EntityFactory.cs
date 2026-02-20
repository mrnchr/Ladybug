using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using Zenject;
using Object = UnityEngine.Object;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public class EntityFactory : IDisposable
  {
    private const int MAX_POOL_SIZE = 10;

    private readonly IInstantiator _instantiator;
    private readonly IEcsUniverse _universe;
    private readonly GameplayUpdater _gameplayUpdater;
    private readonly FacadeEcsConverter _facadeEcsConverter;
    private readonly CreationContext _cachedContext = new CreationContext();
    
    private readonly Dictionary<Type, Stack<EntityInitContext>> _initContextPools =
      new Dictionary<Type, Stack<EntityInitContext>>();

    private readonly Dictionary<EntityType, RecipeHandle> _recipes = new Dictionary<EntityType, RecipeHandle>();
    private readonly Dictionary<Type, InitializerHandle> _initializers = new Dictionary<Type, InitializerHandle>();

    public EntityFactory(IInstantiator instantiator, IEcsUniverse universe, GameplayUpdater gameplayUpdater)
    {
      _instantiator = instantiator;
      _universe = universe;
      _gameplayUpdater = gameplayUpdater;
      _facadeEcsConverter = new FacadeEcsConverter();
    }

    public IDisposable RegisterRecipe(CreationRecipe creationRecipe)
    {
      if (_recipes.ContainsKey(creationRecipe.EntityType))
      {
        return null;
      }

      var handle = new RecipeHandle(() => UnregisterRecipe(creationRecipe.EntityType));
      handle.RegisteredRecipe = creationRecipe.Clone();
      _recipes[creationRecipe.EntityType] = handle;
      return handle;
    }

    public IDisposable RegisterInitializer<TInitializer>() where TInitializer : IEntityInitializer
    {
      Type initializerType = typeof(TInitializer);

      if (_initializers.ContainsKey(initializerType))
      {
        return null;
      }

      var handle = new InitializerHandle(UnregisterInitializer<TInitializer>);
      handle.RegisteredInitializer = _instantiator.Instantiate<TInitializer>();
      _initializers[initializerType] = handle;
      return handle;
    }

    public EcsEntityWrapper CreateEntity(EntityType entityType)
    {
      var createdEntity = new EcsEntityWrapper();
      CreateEntity(entityType, createdEntity);
      return createdEntity;
    }

    public EcsEntityWrapper CreateEntity<TContext>(EntityType entityType, TContext initContext) where TContext : struct
    {
      var createdEntity = new EcsEntityWrapper();
      CreateEntity(entityType, createdEntity, initContext);
      return createdEntity;
    }

    public void CreateEntity(EntityType entityType, EcsEntityWrapper entity)
    {
      _cachedContext.Reset(entityType, entity);
      CreateEntity(_cachedContext);
    }

    public void CreateEntity<TContext>(EntityType entityType, EcsEntityWrapper entity, TContext initContext)
      where TContext : struct
    {
      EntityInitContext context = Get<TContext>();
      context.Set(initContext);
      _cachedContext.Reset(entityType, entity, initContext: context);
      CreateEntity(_cachedContext);
      Release<TContext>(context);
    }

    public EntityVisual CreateVisual(EntityType entityType)
    {
      var createdEntity = new EcsEntityWrapper();
      CreateEntity(entityType, createdEntity);
      return _cachedContext.Visual;
    }

    public EntityVisual CreateVisual<TContext>(EntityType entityType, TContext initContext) where TContext : struct
    {
      var createdEntity = new EcsEntityWrapper();
      CreateEntity(entityType, createdEntity, initContext);
      return _cachedContext.Visual;
    }

    public EcsEntityWrapper CreateEntityWithVisual(EntityType entityType, EntityVisual existingVisual)
    {
      var createdEntity = new EcsEntityWrapper();
      CreateEntityWithVisual(entityType, existingVisual, createdEntity);
      return createdEntity;
    }

    public EcsEntityWrapper CreateEntityWithVisual<TContext>(EntityType entityType,
      EntityVisual existingVisual,
      TContext initContext) where TContext : struct
    {
      var createdEntity = new EcsEntityWrapper();
      CreateEntityWithVisual(entityType, existingVisual, createdEntity, initContext);
      return createdEntity;
    }

    public void CreateEntityWithVisual(EntityType entityType,
      EntityVisual existingVisual,
      EcsEntityWrapper entity)
    {
      _cachedContext.Reset(entityType, entity, visual: existingVisual);
      CreateEntity(_cachedContext);
    }

    public void CreateEntityWithVisual<TContext>(EntityType entityType,
      EntityVisual existingVisual,
      EcsEntityWrapper entity,
      TContext initContext) where TContext : struct
    {
      EntityInitContext context = Get<TContext>();
      context.Set(initContext);
      _cachedContext.Reset(entityType, entity, visual: existingVisual, initContext: context);
      CreateEntity(_cachedContext);
      Release<TContext>(context);
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

    public void Dispose()
    {
      foreach (InitializerHandle handle in _initializers.Values)
      {
        (handle.RegisteredInitializer as IDisposable)?.Dispose();
      }
    }

    private void UnregisterRecipe(EntityType entityType)
    {
      _recipes.Remove(entityType);
    }

    private void UnregisterInitializer<TInitializer>() where TInitializer : IEntityInitializer
    {
      Type initializerType = typeof(TInitializer);

      if (_initializers.TryGetValue(initializerType, out InitializerHandle handle))
      {
        (handle.RegisteredInitializer as IDisposable)?.Dispose();
        _initializers.Remove(initializerType);
      }
    }

    private void CreateEntity(CreationContext context)
    {
      EcsEntityWrapper entity = context.Entity;
      entity.SetWorld(null);

      CreationRecipe recipe = null;

      if (context.Recipe != null)
      {
        recipe = context.Recipe;
      }
      else if (_recipes.TryGetValue(context.EntityType, out RecipeHandle handle))
      {
        recipe = handle.RegisteredRecipe;
      }

      if (recipe == null)
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

      foreach (InitializerHandle handle in _initializers.Values)
      {
        handle.RegisteredInitializer.InitializeEntity(context);
      }
      
      (context.Facade as IEntityInitializable)?.Initialize(context.InitContext);
      (context.Visual as IEntityInitializable)?.Initialize(context.InitContext);
    }
    
    private EntityInitContext Get<TContextValue>() where TContextValue : struct
    {
      EntityInitContext contextInstance;
      
      if (!_initContextPools.TryGetValue(typeof(TContextValue), out Stack<EntityInitContext> initContexts)
        || initContexts.Count == 0)
      {
        contextInstance = new EntityInitContext();
      }
      else
      {
        contextInstance = initContexts.Pop();
      }

      contextInstance.SetDefault<TContextValue>();
      return contextInstance;
    }

    private void Release<TContextValue>(EntityInitContext initContext) where TContextValue : struct
    {
      Type contextValueType = typeof(TContextValue);
      
      if (!_initContextPools.TryGetValue(contextValueType, out Stack<EntityInitContext> initContexts))
      {
        initContexts = new Stack<EntityInitContext>();
        _initContextPools[contextValueType] = initContexts;
      }

      if (initContexts.Count >= MAX_POOL_SIZE)
      {
        return;
      }

      initContext.SetDefault<TContextValue>();
      initContexts.Push(initContext);
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