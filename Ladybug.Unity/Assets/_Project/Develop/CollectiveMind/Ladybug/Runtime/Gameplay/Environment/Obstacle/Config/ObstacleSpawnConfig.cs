﻿using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [CreateAssetMenu(menuName = CAC.Names.OBSTACLE_SPAWN_CONFIG_MENU,
    fileName = CAC.Names.OBSTACLE_SPAWN_CONFIG_FILE)]
  public class ObstacleSpawnConfig : ScriptableObject
  {
    public Vector2 SpawnTime;
    public Vector2 SpawnDistance;
    public float DistanceBetweenObstacles;

    [ValidateInput("ValidateChances")]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, Draggable = false)]
    public List<SpawnChance> SpawnChances = CreateSpawnChances();
    
    private static List<SpawnChance> CreateSpawnChances()
    {
      return SpawnableEntities()
        .Select(x => new SpawnChance { EntityType = x })
        .ToList();
    }

    private static IEnumerable<EntityType> SpawnableEntities()
    {
      return Enum.GetValues(typeof(EntityType))
        .Cast<EntityType>()
        .Where(x => x is >= EntityType.Blob and <= EntityType.PushPin2);
    }

#if UNITY_EDITOR
    private TriValidationResult ValidateChances()
    {
      float sum = SpawnChances.Sum(x => x.Chance);
      return sum switch
      {
        < 0 or > 1 => TriValidationResult.Error($"Sum of chances is {sum}. It must be between 0 and 1"),
        < 1 => TriValidationResult.Warning($"Sum of chances is {sum}. It is less than 1"),
        _ => TriValidationResult.Info($"Sum of chances is {sum}")
      };
    }
#endif
  }

  [Serializable]
  [DeclareHorizontalGroup(nameof(SpawnChance))]
  public struct SpawnChance
  {
    [GroupNext(nameof(SpawnChance))]
    [HideLabel]
    [DisplayAsString]
    public EntityType EntityType;

    [HideLabel]
    public float Chance;
  }
}