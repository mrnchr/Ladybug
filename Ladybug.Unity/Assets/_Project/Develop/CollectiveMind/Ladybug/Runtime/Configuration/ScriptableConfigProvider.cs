using System.Collections.Generic;
using System.Linq;
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
      return (TConfig) _configs.First(x => x is TConfig);
    }
  }
}