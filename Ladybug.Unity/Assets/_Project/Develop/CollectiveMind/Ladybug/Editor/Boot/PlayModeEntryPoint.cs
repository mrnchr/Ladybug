using CollectiveMind.Ladybug.Editor.Boot.Installers;
using CollectiveMind.Ladybug.Runtime;
using UnityEngine;

namespace CollectiveMind.Ladybug.Editor.Boot
{
  public class PlayModeEntryPoint
  {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Entry()
    {
      EditorBridge.OnProjectInstalled += EditorProjectInstaller.Install;
      EditorBridge.OnLevelInstalled += EditorLevelInstaller.Install;
    }
  }
}