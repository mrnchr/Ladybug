using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public class BaseWindow : MonoBehaviour
  {
    public bool IsOpen { get; private set; }

    public bool IsShown { get; private set; }

    public bool IsCovered { get; private set; }

    public async UniTask Open()
    {
      IsOpen = true;
      ShowInternal();
      await OnOpened();
    }

    public async UniTask Close()
    {
      IsOpen = false;
      HideInternal();
      await OnClosed();
    }

    public async UniTask Show()
    {
      ShowInternal();
      await OnShowed();
    }

    public async UniTask Hide()
    {
      HideInternal();
      await OnHid();
    }

    public async UniTask Cover()
    {
      IsCovered = true;
      await OnCovered();
    }

    public async UniTask Reveal()
    {
      IsCovered = false;
      await OnRevealed();
    }

    private void ShowInternal()
    {
      IsShown = true;
      gameObject.SetActive(true);
      AddListeners();
    }

    private void HideInternal()
    {
      IsShown = false;
      gameObject.SetActive(false);
      RemoveListeners();
    }

    protected virtual async UniTask OnOpened()
    {
      await UniTask.CompletedTask;
    }

    protected virtual async UniTask OnClosed()
    {
      await UniTask.CompletedTask;
    }

    protected virtual async UniTask OnShowed()
    {
      await UniTask.CompletedTask;
    }

    protected virtual async UniTask OnHid()
    {
      await UniTask.CompletedTask;
    }

    protected virtual async UniTask OnCovered()
    {
      await UniTask.CompletedTask;
    }

    protected virtual async UniTask OnRevealed()
    {
      await UniTask.CompletedTask;
    }

    protected virtual void AddListeners()
    {
    }

    protected virtual void RemoveListeners()
    {
    }
  }
}