using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using JetBrains.Annotations;
using TriInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [CreateAssetMenu(menuName = CAC.CONFIG_MENU + nameof(EcsConvertersProvider),
    fileName = nameof(EcsConvertersProvider))]
  public class EcsConvertersProvider : ScriptableObject
  {
    [HideInInspector]
    public List<EcsConverterEntry> EcsConverters = new List<EcsConverterEntry>();

    public IEcsConverter Get(EntityType type)
    {
      return EcsConverters.Find(x => x.Type == type).Converter;
    }

#if UNITY_EDITOR
    private readonly List<EcsConverterEntry> _visualEcsConverters = new List<EcsConverterEntry>();
    
    private bool _sortByName = true;
    private bool _sortById;
    private bool _needRedraw = true;

    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    [GUIColor(CC.CYAN)]
    private void SortByName()
    {
      _sortByName = true;
      _sortById = false;
      _needRedraw = true;
    }

    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    [GUIColor(CC.CYAN)]
    private void SortById()
    {
      _sortByName = false;
      _sortById = true;
      _needRedraw = true;
    }

    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    [GUIColor(CC.YELLOW)]
    private void SyncWithEntityTypes(SideType addTo = SideType.Top)
    {
      List<EcsConverterEntry> updatedList = ListPool<EcsConverterEntry>.Get();
      List<EcsConverterEntry> addedEntries = ListPool<EcsConverterEntry>.Get();

      try
      {
        updatedList.Clear();
        addedEntries.Clear();

        foreach (EntityType entity in EnumRange<EntityType>.AllValues)
        {
          if (entity != EntityType.None)
          {
            int entryIndex = EcsConverters.FindIndex(x => x.Type == entity);

            if (entryIndex != -1)
            {
              updatedList.Add(EcsConverters[entryIndex]);
            }
            else
            {
              var newEntry = new EcsConverterEntry { Type = entity };
              updatedList.Add(newEntry);
              addedEntries.Add(newEntry);
            }
          }
        }

        EcsConverters.Clear();
        EcsConverters.AddRange(updatedList);

        _visualEcsConverters.Clear();
        _visualEcsConverters.AddRange(updatedList);
        SortConverters(_visualEcsConverters);

        if (addTo != SideType.None && addedEntries.Count > 0)
        {
          SortConverters(addedEntries);
          int addIndex = addTo == SideType.Top ? 0 : EcsConverters.Count - 1;

          foreach (EcsConverterEntry entry in addedEntries)
          {
            _visualEcsConverters.Remove(entry);
            _visualEcsConverters.Insert(addIndex, entry);
            
            if (addTo == SideType.Top)
            {
              addIndex++;
            }
          }
        }
      }
      finally
      {
        ListPool<EcsConverterEntry>.Release(updatedList);
        ListPool<EcsConverterEntry>.Release(addedEntries);
      }
    }

    [ListDrawerSettings(AlwaysExpanded = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    [ShowInInspector]
    [HideReferencePicker]
    public List<EcsConverterEntry> VisualEcsConverters
    {
      get
      {
        if (_visualEcsConverters.Count != EcsConverters.Count)
        {
          _visualEcsConverters.Clear();
          _visualEcsConverters.AddRange(EcsConverters);
          _needRedraw = true;
        }
        
        if (_needRedraw)
        {
          SortConverters(_visualEcsConverters);
          _needRedraw = false;
        }

        return _visualEcsConverters;
      }
      set { }
    }

    private void SortConverters(List<EcsConverterEntry> list)
    {
      if (_sortByName)
      {
        list.Sort((a, b) =>
          string.Compare(a.Type.ToString(), b.Type.ToString(), StringComparison.Ordinal));
      }

      if (_sortById)
      {
        list.Sort((a, b) => a.Type.CompareTo(b.Type));
      }
    }
#endif
  }

  [Serializable]
  [DeclareHorizontalGroup(nameof(EcsConverterEntry))]
  public class EcsConverterEntry
  {
    [HideInInspector]
    public EntityType Type;

    [Group(nameof(EcsConverterEntry))]
    [HideLabel]
    public EcsConverterAsset Converter;

#if UNITY_EDITOR
    [Group(nameof(EcsConverterEntry))]
    [HideLabel]
    [PropertyOrder(0)]
    [ShowInInspector]
    public EntityType VisualType
    {
      get => Type;
      set { }
    }
#endif
  }
}