using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading
{
  [CreateAssetMenu(menuName = CAC.Names.SCENES_PROVIDER_MENU, fileName = CAC.Names.SCENES_PROVIDER_FILE)]
  public class ScenesProvider : ScriptableObject
  {
    [SerializeField]
    private List<SceneTuple> _scenes;

    public string GetSceneName(SceneType id)
    {
      return _scenes.Find(x => x.Id == id)?.Name;
    }
  }

  [Serializable]
  [DeclareHorizontalGroup(nameof(SceneTuple))]
  public class SceneTuple
  {
    [GroupNext(nameof(SceneTuple))]
    [HideLabel]
    public SceneType Id;
    
    [Scene]
    [HideLabel]
    public string Name;
  }
}