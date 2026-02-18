using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using JetBrains.Annotations;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [CreateAssetMenu(menuName = CAC.CONFIG_MENU + nameof(EcsConvertersProvider), fileName = nameof(EcsConvertersProvider))]
  public class EcsConvertersProvider : ScriptableObject
  {
    [ListDrawerSettings(AlwaysExpanded = true)]
    public List<EcsConverterEntry> EcsConverters;

    public IEcsConverter Get(EntityType type)
    {
      return EcsConverters.Find(x => x.Type == type).Converter;
    }

#if UNITY_EDITOR
    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    private void SortByName()
    {
      EcsConverters.Sort((a, b) => string.Compare(a.Type.ToString(), b.Type.ToString(), StringComparison.Ordinal));
    }

    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    private void SortById()
    {
      EcsConverters.Sort((a, b) => a.Type.CompareTo(b.Type));
    }

    [Button]
    [PropertyOrder(0)]
    [UsedImplicitly]
    private void AddMissingValues(bool toTop = true)
    {
      foreach (EntityType entity in EnumRange<EntityType>.AllRange)
      {
        if (entity != EntityType.None && !EcsConverters.Exists(x => x.Type == entity))
        {
          EcsConverters.Insert(toTop ? 0 : EcsConverters.Count, new EcsConverterEntry { Type = entity });
        }
      }
    }
#endif
  }

  [Serializable]
  [DeclareHorizontalGroup(nameof(EcsConverterEntry))]
  public struct EcsConverterEntry
  {
    [Group(nameof(EcsConverterEntry))]
    [HideLabel]
    public EntityType Type;

    [Group(nameof(EcsConverterEntry))]
    [HideLabel]
    public EcsConverterAsset Converter;
  }
}