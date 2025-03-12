using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  public static class CollisionServiceExtensions
  {
    public static bool TrySelectByComponents<TMasterComponent, TTargetComponent>(this ICollisionService obj) 
      where TMasterComponent : struct, IEcsComponent where TTargetComponent : struct, IEcsComponent
    {
      return obj.TrySelectByEntities(x => x.Has<TMasterComponent>(), x => x.Has<TTargetComponent>());
    }
  }
}