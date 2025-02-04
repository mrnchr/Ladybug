using System.Collections.Generic;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Configuration
{
  [CreateAssetMenu(menuName = CAC.Names.SCRIPTABLE_CONFIG_PROVIDER_MENU,
    fileName = CAC.Names.SCRIPTABLE_CONFIG_PROVIDER_FILE)]
  public class ScriptableConfigProvider : ScriptableObject, IConfigProvider
  {
    [SerializeField] 
    private List<ScriptableObject> _configs = new List<ScriptableObject>();
    
    public TConfig Get<TConfig>() where TConfig : ScriptableObject
    {
      return _configs.Find(x => x is TConfig) as TConfig;
    }
  }
}