using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public class WindowManager : IWindowManager
  {
    private readonly List<BaseWindow> _windows = new List<BaseWindow>();
    private readonly Stack<BaseWindow> _history = new Stack<BaseWindow>();

    public void AddWindow(BaseWindow window)
    {
      _windows.Add(window);
    }

    public TWindow GetWindow<TWindow>() where TWindow : BaseWindow
    {
      return _windows.Find(x => x is TWindow) as TWindow;
    }

    public void RemoveWindow(BaseWindow window)
    {
      _windows.Remove(window);
    }
    
    public async UniTask<TWindow> OpenWindowAsRoot<TWindow>() where TWindow : BaseWindow
    {
      while (_history.Count > 0)
        CloseLastOpenedWindow().Forget();
      
      var window = GetWindow<TWindow>();
      if (window)
      {
        _history.Push(window);
        await window.Open();
      }

      return window;
    }

    public async UniTask<TWindow> OpenWindow<TWindow>(bool hide = true) where TWindow : BaseWindow
    {
      if (_history.TryPeek(out BaseWindow lastWindow))
        await (hide ? lastWindow.Hide() : lastWindow.Cover());

      var window = GetWindow<TWindow>();
      _history.Push(window);
      await window.Open();
      return window;
    }

    public async UniTask CloseLastOpenedWindow()
    {
      await PopAndCloseLastOpenedWindow();
      await DisplayLastWindow();
    }

    public async UniTask CloseWindow<TWindow>() where TWindow : BaseWindow
    {
      if (_history.Peek() is not TWindow)
        return;

      await PopAndCloseLastOpenedWindow();

      await DisplayLastWindow();
    }

    public async UniTask CloseWindowsBy<TWindow>() where TWindow : BaseWindow
    {
      if (!_history.Any(x => x is TWindow))
        return;

      while (_history.Peek() is not TWindow)
      {
        await PopAndCloseLastOpenedWindow();
      }

      await PopAndCloseLastOpenedWindow();
      await DisplayLastWindow();
    }

    private async UniTask DisplayLastWindow()
    {
      if (_history.TryPeek(out BaseWindow nextWindow))
        await (nextWindow.IsCovered ? nextWindow.Cover() : nextWindow.Show());
    }

    private async UniTask PopAndCloseLastOpenedWindow()
    {
      await _history.Pop().Close();
    }
  }
}