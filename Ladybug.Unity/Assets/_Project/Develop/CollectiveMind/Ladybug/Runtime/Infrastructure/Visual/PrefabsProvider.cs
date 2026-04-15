using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using JetBrains.Annotations;
using TriInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  [CreateAssetMenu(menuName = CAC.CONFIG_MENU + nameof(PrefabsProvider), fileName = nameof(PrefabsProvider))]
  public class PrefabsProvider : ScriptableObject
  {
    [HideInInspector]
    public List<ViewPrefabEntry> Prefabs = new List<ViewPrefabEntry>();

    public EntityVisual Get(EntityType id)
    {
      return Prefabs.Find(x => x.Id == id).Prefab;
    }

#if UNITY_EDITOR
    private readonly List<ViewPrefabEntry> _visualPrefabs = new List<ViewPrefabEntry>();

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
      List<ViewPrefabEntry> updatedList = ListPool<ViewPrefabEntry>.Get();
      List<ViewPrefabEntry> addedEntries = ListPool<ViewPrefabEntry>.Get();

      try
      {
        updatedList.Clear();
        addedEntries.Clear();

        foreach (EntityType entity in EnumRange<EntityType>.AllValues)
        {
          if (entity != EntityType.None)
          {
            int entryIndex = Prefabs.FindIndex(x => x.Id == entity);

            if (entryIndex != -1)
            {
              updatedList.Add(Prefabs[entryIndex]);
            }
            else
            {
              var newEntry = new ViewPrefabEntry { Id = entity };
              updatedList.Add(newEntry);
              addedEntries.Add(newEntry);
            }
          }
        }

        Prefabs.Clear();
        Prefabs.AddRange(updatedList);

        _visualPrefabs.Clear();
        _visualPrefabs.AddRange(updatedList);
        SortConverters(_visualPrefabs);

        if (addTo != SideType.None && addedEntries.Count > 0)
        {
          SortConverters(addedEntries);
          int addIndex = addTo == SideType.Top ? 0 : Prefabs.Count - 1;

          foreach (ViewPrefabEntry entry in addedEntries)
          {
            _visualPrefabs.Remove(entry);
            _visualPrefabs.Insert(addIndex, entry);

            if (addTo == SideType.Top)
            {
              addIndex++;
            }
          }
        }
      }
      finally
      {
        ListPool<ViewPrefabEntry>.Release(updatedList);
        ListPool<ViewPrefabEntry>.Release(addedEntries);
      }
    }

    [ListDrawerSettings(AlwaysExpanded = true, Draggable = false, HideAddButton = true, HideRemoveButton = true)]
    [ShowInInspector]
    [HideReferencePicker]
    public List<ViewPrefabEntry> VisualPrefabs
    {
      get
      {
        if (_visualPrefabs.Count != Prefabs.Count)
        {
          _visualPrefabs.Clear();
          _visualPrefabs.AddRange(Prefabs);
          _needRedraw = true;
        }

        if (_needRedraw)
        {
          SortConverters(_visualPrefabs);
          _needRedraw = false;
        }

        return _visualPrefabs;
      }
      set { }
    }

    private void SortConverters(List<ViewPrefabEntry> list)
    {
      if (_sortByName)
      {
        list.Sort((a, b) =>
          string.Compare(a.Id.ToString(), b.Id.ToString(), StringComparison.Ordinal));
      }

      if (_sortById)
      {
        list.Sort((a, b) => a.Id.CompareTo(b.Id));
      }
    }
#endif
  }

  [Serializable]
  [DeclareHorizontalGroup(nameof(ViewPrefabEntry))]
  public class ViewPrefabEntry
  {
    [HideInInspector]
    public EntityType Id;

    [Group(nameof(ViewPrefabEntry))]
    [HideLabel]
    public EntityVisual Prefab;

#if UNITY_EDITOR
    [Group(nameof(ViewPrefabEntry))]
    [HideLabel]
    [PropertyOrder(0)]
    [ShowInInspector]
    public EntityType VisualId
    {
      get => Id;
      set { }
    }
#endif
  }
}