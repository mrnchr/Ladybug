using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using JetBrains.Annotations;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  [CreateAssetMenu(menuName = CAC.CONFIG_MENU + nameof(PrefabsProvider), fileName = nameof(PrefabsProvider))]
  public class PrefabsProvider : ScriptableObject
  {
    [ListDrawerSettings(AlwaysExpanded = true)]
    public List<ViewPrefabTuple> Prefabs;

    public EntityVisual Get(EntityType id)
    {
      return Prefabs.Find(x => x.Id == id).Prefab;
    }
    
#if UNITY_EDITOR
    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    private void SortByName()
    {
      Prefabs.Sort((a, b) => string.Compare(a.Id.ToString(), b.Id.ToString(), StringComparison.Ordinal));
    }

    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    private void SortById()
    {
      Prefabs.Sort((a, b) => a.Id.CompareTo(b.Id));
    }

    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    private void AddMissingValues(bool toTop = true)
    {
      foreach (EntityType entity in EnumRange<EntityType>.AllRange)
      {
        if (entity != EntityType.None && !Prefabs.Exists(x => x.Id == entity))
        {
          Prefabs.Insert(toTop ? 0 : Prefabs.Count, new ViewPrefabTuple { Id = entity });
        }
      }
    }
#endif
  }
  
  [Serializable]
  [DeclareHorizontalGroup(nameof(ViewPrefabTuple))]
  public struct ViewPrefabTuple
  {
    [Group(nameof(ViewPrefabTuple))]
    [HideLabel]
    public EntityType Id;

    [Group(nameof(ViewPrefabTuple))]
    [HideLabel]
    public EntityVisual Prefab;
  }
}