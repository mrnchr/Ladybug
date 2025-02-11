using Leopotam.EcsLite;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsEngine : IInitializable, IFixedTickable, ITickable, ILateTickable
  {
    private readonly EcsSystems _fixedUpdateSystems;
    private readonly EcsSystems _updateSystems;
    private readonly EcsSystems _lateUpdateSystems;

    public EcsEngine(IEcsSystemsFactory systemsFactory, IEcsSystemFactory factory)
    {
      _fixedUpdateSystems = systemsFactory.Create(EcsConstants.Worlds.GAME);
      _updateSystems = systemsFactory.Create(EcsConstants.Worlds.GAME);
      _lateUpdateSystems = systemsFactory.Create(EcsConstants.Worlds.GAME);

      _fixedUpdateSystems
        .Add(factory.Create<FixedUpdateFeature>());

      _updateSystems
        .Add(factory.Create<UpdateFeature>());

      _lateUpdateSystems
        .Add(factory.Create<LateUpdateFeature>());
    }

    public void Initialize()
    {
      _fixedUpdateSystems.Init();
      _updateSystems.Init();
      _lateUpdateSystems.Init();
    }

    public void FixedTick()
    {
      _fixedUpdateSystems.Run();
    }

    public void Tick()
    {
      _updateSystems.Run();
    }

    public void LateTick()
    {
      _lateUpdateSystems.Run();
    }
  }
}