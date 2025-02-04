using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Configuration
{
  public interface IConfigProvider
  {
    public TConfig Get<TConfig>() where TConfig : ScriptableObject;
  }
}