#if UNITY_EDITOR
using System;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime
{
  public static class EditorBridge
  {
    public static event Action<DiContainer> OnProjectInstalled;
    public static event Action<DiContainer> OnLevelInstalled;

    public static void InstallProject(DiContainer container)
    {
      OnProjectInstalled?.Invoke(container);
    }

    public static void InstallGameplay(DiContainer container)
    {
      OnLevelInstalled?.Invoke(container);
    }
  }
}
#endif