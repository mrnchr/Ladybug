using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public class EntityVisual : MonoBehaviour, IEcsConverter
  {
    public EcsEntityWrapper Entity { get; } = new EcsEntityWrapper();
    
    [SerializeField]
    private EcsGameObjectBinder _ecsBinder;

    private EntityFactory _entityFactory;

    [Inject]
    private void Construct(EntityFactory entityFactory)
    {
      _entityFactory = entityFactory;
    }

    public void AttachEntity(EcsEntityWrapper entity)
    {
      if (!_ecsBinder)
      {
        return;
      }

      Entity.Copy(entity);
      _ecsBinder.Bind(Entity);
    }

    public void DetachEntity()
    {
      if (Entity.IsAlive() && Entity.Has<EntityVisualRef>())
      {
        _ecsBinder.ConvertBack(Entity);
      }
    }

    public virtual void OnPreDestroy()
    {
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref EntityVisualRef entityVisualRef) => entityVisualRef.Visual = this);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<EntityVisualRef>();
    }

    private void OnDestroy()
    {
      _entityFactory.DestroyEntity(Entity);
    }

#if UNITY_EDITOR
    [ShowInInspector]
    [UsedImplicitly]
    private EcsPackedEntity _packedEntity
    {
      get => Entity != null && Entity.IsAlive() ? Entity.PackedEntity : default(EcsPackedEntity);
      set { }
    }
    
    private void Reset()
    {
      TryGetComponent(out _ecsBinder);
    }
#endif
  }
}