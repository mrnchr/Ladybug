using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public class CollisionsInstaller : Installer<CollisionsInstaller>
  {
    public override void InstallBindings()
    {
      BindCollisionFiller();
      BindCollisionPacker();
      BindCollisionService();
    }

    private void BindCollisionFiller()
    {
      Container
        .Bind<ICollisionFiller>()
        .To<CollisionFiller>()
        .AsSingle();
    }

    private void BindCollisionPacker()
    {
      Container
        .Bind<ICollisionPacker>()
        .To<CollisionPacker>()
        .AsSingle();
    }

    private void BindCollisionService()
    {
      Container
        .Bind<ICollisionFilter>()
        .To<CollisionFilter>()
        .AsSingle();
    }
  }
}