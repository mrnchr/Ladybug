using CollectiveMind.Ladybug.Runtime.Infrastructure;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public static class EntityTypeUtils
  {
    public static readonly EntityType StartObstacle = EntityType.Blob;
    public static readonly EntityType EndObstacleExclusive = EntityType.Marker;

    public static readonly EnumRange<EntityType> Obstacles = new EnumRange<EntityType>(StartObstacle, EndObstacleExclusive);

    public static bool IsObstacle(EntityType type)
    {
      return StartObstacle <= type && type < EndObstacleExclusive;
    }
  }
}