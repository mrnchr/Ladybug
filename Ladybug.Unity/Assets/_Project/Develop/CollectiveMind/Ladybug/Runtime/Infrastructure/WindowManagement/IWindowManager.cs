using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public interface IWindowManager
  {
    void AddWindow(BaseWindow window);
    TWindow GetWindow<TWindow>() where TWindow : BaseWindow;
    void RemoveWindow(BaseWindow window);
    UniTask<TWindow> OpenWindowAsRoot<TWindow>() where TWindow : BaseWindow;
    UniTask<TWindow> OpenWindow<TWindow>(bool hide = true) where TWindow : BaseWindow;
    UniTask<TWindow> CloseWindow<TWindow>() where TWindow : BaseWindow;
    UniTask<TWindow> CloseWindowsBy<TWindow>() where TWindow : BaseWindow;
  }
}