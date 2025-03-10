using System.Collections.Generic;
using System.Linq;
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
      if(_history.TryPeek(out WindowBase lastWindow))
        await lastWindow.Hide();
      
      var window = GetWindow<TWindow>();
      _history.Push(window);
      await window.Open();
      return window;
    }

    public async UniTask<TWindow> CloseWindow<TWindow>() where TWindow : WindowBase
    {
      if (_history.Peek() is not TWindow)
        return null;

      await CloseLastWindow();
      
      return await ShowLastWindow<TWindow>();
    }

    public async UniTask<TWindow> CloseWindowsBy<TWindow>() where TWindow : WindowBase
    {
      if (!_history.Any(x => x is TWindow))
        return null;

      while (_history.Peek() is not TWindow)
      {
        await CloseLastWindow();
      }

      await CloseLastWindow();
      
      return await ShowLastWindow<TWindow>();
    }

    private async UniTask<TWindow> ShowLastWindow<TWindow>() where TWindow : WindowBase
    {
      if (_history.TryPeek(out WindowBase nextWindow))
        await nextWindow.Show();
      
      return nextWindow as TWindow;
    }

    private async UniTask CloseLastWindow()
    {
      await _history.Pop().Close();
    }
  }
}