using System;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public class RecipeHandle : IDisposable
  {
    public CreationRecipe RegisteredRecipe;
    
    private readonly Action _onUnregister;
    private bool _unregistered;

    public RecipeHandle(Action onUnregister)
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