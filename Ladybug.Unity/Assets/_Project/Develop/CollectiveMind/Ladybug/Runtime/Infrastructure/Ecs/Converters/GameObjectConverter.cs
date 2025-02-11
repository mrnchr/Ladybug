using System.Collections.Generic;
using System.Linq;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [RequireComponent(typeof(MonoInjector))]
  public class GameObjectConverter : MonoBehaviour, IGameObjectConverter, IInitializable
  {
    private readonly EcsEntityWrapper _entityWrapper = new EcsEntityWrapper();
    
    [SerializeField]
    private List<EcsConverterValue> _converters;

    [SerializeField]
    private bool _createEntityOnStart = true;

    private EcsWorld _message;
    private List<IEcsConverter> _viewConverters;

    public bool ShouldCreateEntity { get; set; } = true;
    public EcsEntityWrapper EntityWrapper => _entityWrapper;

    public bool CreateEntityOnStart
    {
      get => _createEntityOnStart;
      set => _createEntityOnStart = value;
    }

    [Inject]
    public void Construct(IRuntimeInitializer phase, MessageWorldWrapper messageWorldWrapper)
    {
      phase.Add(this);

      _message = messageWorldWrapper.World;
      GetConvertersInChildren();
    }

    private void GetConvertersInChildren()
    {
      _viewConverters = GetComponents<IEcsConverter>().ToList();
      for (int i = 0; i < transform.childCount; i++)
        GetConverters(transform.GetChild(i), _viewConverters);

      _viewConverters.Remove(this);
    }

    private void GetConverters(Transform t, List<IEcsConverter> converters)
    {
      IEcsConverter[] list = t.GetComponents<IEcsConverter>();
      if (list.Any(x => x is GameObjectConverter))
        return;

      converters.AddRange(list);
      for (int i = 0; i < t.childCount; i++)
        GetConverters(t.GetChild(i), converters);
    }

    public void Initialize()
    {
      if (ShouldCreateEntity && CreateEntityOnStart)
        SendCreateMessage();
    }

    public void SendCreateMessage()
    {
      _message.CreateEntity()
        .Add((ref CreateEntityMessage createMessage) =>
        {
          createMessage.Entity = _entityWrapper.PackedEntity;
          createMessage.Converter = this;
        });
    }

    public void CreateEntity()
    {
      foreach (EcsConverterValue converter in _converters)
        converter.ConvertTo(_entityWrapper);

      ConvertTo(_entityWrapper);
    }

    public void CreateEntity(EcsEntityWrapper entity)
    {
      _entityWrapper.Copy(entity);
      CreateEntity();
    }

    public void ConvertBackAndDestroy(EcsEntityWrapper entity)
    {
      ConvertBack(entity);
      Destroy(gameObject);
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      foreach (IEcsConverter converter in _viewConverters)
        converter.ConvertTo(entity);

      entity.Add((ref ConverterRef converterRef) => converterRef.Converter = this);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      foreach (IEcsConverter converter in _viewConverters)
        converter.ConvertBack(entity);

      entity.Del<ConverterRef>();
    }
    
    public void SetEntity(EcsEntityWrapper entity)
    {
      _entityWrapper.Copy(entity);
    }

    public void SetEntity(EcsWorld world, int entity)
    {
      _entityWrapper.SetWorld(world, entity);
    }
  }
}