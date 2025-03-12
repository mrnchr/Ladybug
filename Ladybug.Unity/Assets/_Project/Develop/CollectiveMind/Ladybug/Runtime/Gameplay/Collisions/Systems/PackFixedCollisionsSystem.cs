using Leopotam.EcsLite;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class PackFixedCollisionsSystem : IEcsRunSystem
  {
    private readonly ICollisionPacker _packer;

    public PackFixedCollisionsSystem(ICollisionPacker packer)
    {
      _packer = packer;
    }

    public void Run(IEcsSystems systems)
    {
      _packer.Pack(true);
    }
  }
}