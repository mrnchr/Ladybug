using System;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  [CreateAssetMenu(menuName = CAC.Names.PREFAB_PROVIDER_MENU, fileName = CAC.Names.PREFAB_PROVIDER_FILE)]
  public class PrefabsProvider : ScriptableObject
  {
    [ListDrawerSettings(AlwaysExpanded = true)]
    public List<ViewPrefabTuple> Prefabs;

    public BaseView Get(EntityType id)
    {
      return Prefabs.Find(x => x.Id == id).Prefab;
    }
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
    public BaseView Prefab;
  }
}