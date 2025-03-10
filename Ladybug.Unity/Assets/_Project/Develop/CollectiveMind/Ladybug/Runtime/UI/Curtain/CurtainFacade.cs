using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.UI.Curtain
{
  public class CurtainFacade
  {
    private readonly CurtainConfig _config;

    public ReactiveProperty<bool> IsVisible { get; } = new ReactiveProperty<bool>();
    public readonly ReadOnlyReactiveProperty<float> Progress;

    public CurtainFacade(SceneLoadingData sceneLoadingData, IConfigProvider configProvider)
    {
      _config = configProvider.Get<CurtainConfig>();

      Progress = sceneLoadingData.Progress
        .Select(x => Mathf.Clamp01(x / 0.9f))
        .ToReadOnlyReactiveProperty();
    }

    public async UniTask ShowCurtain()
    {
      IsVisible.Value = true;
      await UniTask.WaitForSeconds(_config.ShowTime);
    }

    public async UniTask HideCurtain()
    {
      IsVisible.Value = false;
      await UniTask.WaitForSeconds(_config.ShowTime);
    }
  }
}