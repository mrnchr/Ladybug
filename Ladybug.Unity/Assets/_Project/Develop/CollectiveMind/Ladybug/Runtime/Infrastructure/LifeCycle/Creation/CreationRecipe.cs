using System;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public class CreationRecipe
  {
    public EntityType EntityType;
    public IEcsConverter EcsConverter;
    public EntityVisual VisualPrefab;
    public Type FacadeType;

    public CreationRecipe()
    {
    }
    
    public CreationRecipe(EntityType entityType, IEcsConverter ecsConverter, EntityVisual visualPrefab, Type facadeType)
    {
      EntityType = entityType;
      EcsConverter = ecsConverter;
      VisualPrefab = visualPrefab;
      FacadeType = facadeType;
    }
    
    public void Replace(EntityType entityType, IEcsConverter ecsConverter, EntityVisual visualPrefab, Type facadeType)
    {
      EntityType = entityType;
      EcsConverter = ecsConverter;
      VisualPrefab = visualPrefab;
      FacadeType = facadeType;
    }

    public CreationRecipe Clone()
    {
      return (CreationRecipe)MemberwiseClone();
    }

    public void Clear()
    {
      EntityType = EntityType.None;
      EcsConverter = null;
      VisualPrefab = null;
      FacadeType = null;
    }
  }
}