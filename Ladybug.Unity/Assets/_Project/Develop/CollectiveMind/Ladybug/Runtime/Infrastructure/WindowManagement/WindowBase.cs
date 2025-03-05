using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public class WindowBase : MonoBehaviour
  {
    public bool IsOpen { get; private set; }

    public async UniTask Open()
    {
      IsOpen = true;
      gameObject.SetActive(true);
      AddListeners();
      await OnOpened();
    }

    public async UniTask Close()
    {
      IsOpen = false;
      gameObject.SetActive(false);
      RemoveListeners();
      await OnClosed();
    }

    protected virtual async UniTask OnOpened()
    {
      await UniTask.CompletedTask;
    }

    protected virtual async UniTask OnClosed()
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