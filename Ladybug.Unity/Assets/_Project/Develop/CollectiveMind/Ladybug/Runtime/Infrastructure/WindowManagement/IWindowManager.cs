using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public interface IWindowManager
  {
    void AddWindow(WindowBase window);
    TWindow GetWindow<TWindow>() where TWindow : WindowBase;
    void RemoveWindow(WindowBase window);
    UniTask<TWindow> OpenWindow<TWindow>() where TWindow : WindowBase;
    UniTask<TWindow> CloseWindow<TWindow>() where TWindow : WindowBase;
  }
}