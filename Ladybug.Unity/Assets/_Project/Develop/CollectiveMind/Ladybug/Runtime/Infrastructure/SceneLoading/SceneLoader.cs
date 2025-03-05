using System;
using CollectiveMind.Ladybug.Runtime.Configuration;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading
{
  public class SceneLoader : ISceneLoader, IDisposable
  {
    private readonly SceneLoadingData _loadingData;
    private readonly ScenesProvider _config;

    private bool _wasSceneChanged;

    public SceneLoader(SceneLoadingData loadingData, IConfigProvider configProvider)
    {
      _loadingData = loadingData;
      _config = configProvider.Get<ScenesProvider>();

      SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene _, Scene __)
    {
      _wasSceneChanged = true;
    }

    public void Dispose()
    {
      SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public async UniTask LoadAsync(SceneType id)
    {
      string sceneName = _config.GetSceneName(id);
      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
      operation.allowSceneActivation = false;
      while (operation.progress < 0.89)
      {
        _loadingData.Progress.Value = operation.progress;
        await UniTask.Yield();
      }
      
      operation.allowSceneActivation = true;
      
      _wasSceneChanged = false;
      await UniTask.WaitUntil(() => _wasSceneChanged);
      
      _loadingData.Progress.Value = 0;
    }
  }
}