using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation
{
  public class CreateEntityWithViewSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly EcsEntities _creationMessages;

    public CreateEntityWithViewSystem(IEcsUniverse universe)
    {
      _universe = universe;

      _creationMessages = _universe
        .FilterMessage<CreateEntityMessage>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper message in _creationMessages)
      {
        ref CreateEntityMessage createMessage = ref message.Get<CreateEntityMessage>();
        if (!createMessage.Entity.TryUnpackEntity(_universe.Game, out EcsEntityWrapper createdEntity))
        {
          createdEntity.Entity = _universe.Game.NewEntity();
        }
        
        createMessage.Converter.CreateEntity(createdEntity);
        createdEntity.Add<OnConverted>();
        
        message.Del<CreateEntityMessage>();
      }
    }
  }
}