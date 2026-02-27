using System;
using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle
{
  [CreateAssetMenu(menuName = CAC.Names.OBSTACLE_SPAWN_CONFIG_MENU, fileName = CAC.Names.OBSTACLE_SPAWN_CONFIG_FILE)]
  public class ObstacleSpawnConfig : ScriptableObject
  {
    public Vector2 SpawnTime;
    public float AngularDeviation;
    public float CameraDiagonalSafetyMultiplier = 1;
    public float MaxObstacleSize;
    public Vector2 DistanceRange;
    public float DistanceBetweenObstacles;
    
    [HideInInspector]
    public List<ObstacleSpawnEntry> SpawnEntries;

#if UNITY_EDITOR
    private List<SpawnDataVisual> _spawnEntryVisuals;
    
    [ShowInInspector]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, Draggable = false)]
    [HideReferencePicker]
    [PropertyOrder(6)]
    [LabelText("Spawn Weights")]
    private List<SpawnDataVisual> SpawnEntryVisuals
    {
      get
      {
        InitializeSpawnEntryVisuals();
        UpdateSpawnEntryVisuals();
        return _spawnEntryVisuals;
      }
      set => SetSpawnEntryVisuals(value);
    }

    private void InitializeSpawnEntryVisuals()
    {
      if (_spawnEntryVisuals == null)
      {
        if (SpawnEntries == null)
        {
          SetSpawnEntryVisuals(CreateSpawnChances());
        }
        else
        {
          _spawnEntryVisuals = new List<SpawnDataVisual>();
            
          foreach (ObstacleSpawnEntry data in SpawnEntries)
          {
            _spawnEntryVisuals.Add(new SpawnDataVisual { Entry = data });
          }
        }
      }
    }

    private void UpdateSpawnEntryVisuals()
    {
      List<EntityType> spawnableEntities = SpawnableEntities().ToList();

      if (spawnableEntities.Count != _spawnEntryVisuals.Count)
      {
        List<SpawnDataVisual> newVisuals = _spawnEntryVisuals.Where(x => spawnableEntities.Contains(x.Entry.EntityType)).ToList();
        IEnumerable<EntityType> restEntities = spawnableEntities.Except(_spawnEntryVisuals.Select(x => x.Entry.EntityType));
        newVisuals.AddRange(restEntities.Select(x => new SpawnDataVisual { Entry = new ObstacleSpawnEntry { EntityType = x } }));
        newVisuals.Sort((a, b) => string.Compare(a.Entry.EntityType.ToString(), b.Entry.EntityType.ToString(), StringComparison.Ordinal));
        SetSpawnEntryVisuals(newVisuals);
      }
    }

    private void SetSpawnEntryVisuals(List<SpawnDataVisual> value)
    {
      _spawnEntryVisuals = value;
      SpawnEntries.Clear();

      foreach (SpawnDataVisual visual in _spawnEntryVisuals)
      {
        SpawnEntries.Add(visual.Entry);
      }
    }

    private List<SpawnChance> _testSpawnChances = new List<SpawnChance>();

    [Title("Test Chances")]
    [ShowInInspector]
    private float _testSpawnScore;
    
    [ShowInInspector]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, Draggable = false)]
    [HideReferencePicker]
    private List<SpawnChance> TestSpawnChances
    {
      get
      {
        _testSpawnChances.Clear();

        var sum = 0f;
        
        foreach (ObstacleSpawnEntry data in SpawnEntries)
        {
          var chance = 0f;
          
          if (data.MinSpawnScore <= _testSpawnScore)
          {
            chance = data.SpawnWeight;
            sum += data.SpawnWeight;
          }
          _testSpawnChances.Add(new SpawnChance { EntityType = data.EntityType, Chance = chance });
        }

        _testSpawnChances.ForEach(x => x.Chance /= sum);
        
        return _testSpawnChances;
      }
      set
      {
      }
    }

    private static List<SpawnDataVisual> CreateSpawnChances()
    {
      return SpawnableEntities()
        .Select(x => new SpawnDataVisual { Entry = new ObstacleSpawnEntry { EntityType = x } })
        .OrderBy(x => x.Entry.EntityType.ToString())
        .ToList();
    }

    private static IEnumerable<EntityType> SpawnableEntities()
    {
      return Enum.GetValues(typeof(EntityType))
        .Cast<EntityType>()
        .Where(EntityTypeUtils.IsObstacle);
    }

    private class SpawnChance
    {
      public EntityType EntityType { get; set; }
      public float Chance { get; set; }

      [LabelText("$" + nameof(_label))]
      [DisplayAsString]
      [ShowInInspector]
      private string ChanceString
      {
        get => $"{Chance:P}";
        set
        {
        }
      }

      private string _label => UnityEditor.ObjectNames.NicifyVariableName(EntityType.ToString());
    }

    private class SpawnDataVisual
    {
      [LabelText("$" + nameof(_label))]
      public ObstacleSpawnEntry Entry;

      private string _label => UnityEditor.ObjectNames.NicifyVariableName(Entry.EntityType.ToString());
    }
#endif
  }

  [Serializable]
  public struct ObstacleSpawnEntry
  {
    [HideInInspector]
    public EntityType EntityType;

    public float MinSpawnScore;
    
    public float SpawnWeight;
  }
}