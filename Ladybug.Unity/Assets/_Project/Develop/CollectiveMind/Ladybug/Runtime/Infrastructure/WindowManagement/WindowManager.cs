using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public class WindowManager : IWindowManager
  {
    private readonly List<WindowBase> _windows = new List<WindowBase>();
    private readonly Stack<WindowBase> _history = new Stack<WindowBase>();

    public void AddWindow(WindowBase window)
    {
      _windows.Add(window);
    }

    public TWindow GetWindow<TWindow>() where TWindow : WindowBase
    {
      return _windows.Find(x => x is TWindow) as TWindow;
    }

    public void RemoveWindow(WindowBase window)
    {
      _windows.Remove(window);
    }

    public async UniTask<TWindow> OpenWindow<TWindow>() where TWindow : WindowBase
    {
      var window = GetWindow<TWindow>();
      _history.Push(window);
      await window.Open();
      return window;
    }

    public async UniTask<TWindow> CloseWindow<TWindow>() where TWindow : WindowBase
    {
      if (_history.Peek() is not TWindow lastWindow)
        return null;
      
      await lastWindow.Close();
      _history.Pop();
      return lastWindow;
    }
  }
}