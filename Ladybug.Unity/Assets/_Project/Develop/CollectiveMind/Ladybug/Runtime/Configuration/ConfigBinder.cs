using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Configuration
{
  [Serializable]
  public class ConfigBinder
  {
    public List<ScriptableObject> Configs;
    
    public void BindConfigs(DiContainer container)
    {
      foreach (ScriptableObject config in Configs)
      {
        if (config)
        {
          container
            .Bind(config.GetType())
            .FromInstance(config)
            .AsSingle();
        }
      }      
    }
  }
}