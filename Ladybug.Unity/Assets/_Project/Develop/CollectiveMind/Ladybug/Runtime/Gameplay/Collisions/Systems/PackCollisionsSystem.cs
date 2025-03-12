using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class PackCollisionsSystem : IEcsRunSystem
  {
    private readonly ICollisionPacker _packer;

    public PackCollisionsSystem(ICollisionPacker packer)
    {
      _packer = packer;
    }

    public void Run(IEcsSystems systems)
    {
      _packer.Pack();
    }
  }
}