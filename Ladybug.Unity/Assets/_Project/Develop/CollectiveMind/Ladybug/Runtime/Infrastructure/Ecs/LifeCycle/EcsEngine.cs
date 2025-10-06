using CollectiveMind.Ladybug.Runtime.Gameplay;
using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class EcsEngine : IGameFixedStep, IGameStep
  {
    private readonly EcsSystems _fixedUpdateSystems;
    private readonly EcsSystems _updateSystems;

    public EcsEngine(IEcsSystemsFactory systemsFactory, IEcsSystemFactory factory)
    {
      _fixedUpdateSystems = systemsFactory.Create(EcsConstants.Worlds.GAME);
      _updateSystems = systemsFactory.Create(EcsConstants.Worlds.GAME);

      _fixedUpdateSystems
        .Add(factory.Create<FixedUpdateFeature>());

      _updateSystems
        .Add(factory.Create<UpdateFeature>());
    }

    public void Initialize()
    {
      _fixedUpdateSystems.Init();
      _updateSystems.Init();
    }

    public void FixedStep()
    {
      _fixedUpdateSystems.Run();
    }

    public void Step()
    {
      _updateSystems.Run();
    }
  }
}