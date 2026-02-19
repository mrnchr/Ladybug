using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public class InitializerHandle : IDisposable
  {
    public IEntityInitializer RegisteredInitializer;
    
    private readonly Action _onUnregister;
    private bool _unregistered;

    public InitializerHandle(Action onUnregister)
    {
      _onUnregister = onUnregister;
    }

    public void Dispose()
    {
      if (_unregistered)
      {
        return;
      }

      _unregistered = true;
      _onUnregister?.Invoke();    
    }
  }
}