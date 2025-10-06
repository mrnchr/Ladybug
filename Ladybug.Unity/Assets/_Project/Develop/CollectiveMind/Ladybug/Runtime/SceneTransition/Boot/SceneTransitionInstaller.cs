﻿using CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.SceneTransition.Boot
{
  public class SceneTransitionInstaller : Installer<SceneTransitionInstaller>
  {
    public override void InstallBindings()
    {
      InstallSceneLoading();
    }

    private void InstallSceneLoading()
    {
      SceneLoadingInstaller.Install(Container);
    }
  }
}