using System.Collections.Generic;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Configuration
{
  [CreateAssetMenu(menuName = CAC.CONFIG_MENU + nameof(ConfigProvider), fileName = nameof(ConfigProvider))]
  public class ConfigProvider : ScriptableObject
  {
    public List<ScriptableObject> Configs;
  }
}