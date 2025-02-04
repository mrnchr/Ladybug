using System;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle
{
  public class LifeCycleBinder : ILifeCycleBinder
  {
    private readonly InitializableManager _initializer;
    private readonly TickableManager _ticker;
    private readonly DisposableManager _disposer;

    public LifeCycleBinder(InitializableManager initializer, TickableManager ticker, DisposableManager disposer)
    {
      _initializer = initializer;
      _ticker = ticker;
      _disposer = disposer;
    }

    public void Bind(object obj, LifeCycleFlags flags = LifeCycleFlags.All)
    {
      if (flags.HasFlag(LifeCycleFlags.Initialize) && obj is IInitializable initializable)
        _initializer.Add(initializable);

      if (flags.HasFlag(LifeCycleFlags.FixedTick) && obj is IFixedTickable fixedTickable)
        _ticker.AddFixed(fixedTickable);

      if (flags.HasFlag(LifeCycleFlags.Tick) && obj is ITickable tickable)
        _ticker.Add(tickable);

      if (flags.HasFlag(LifeCycleFlags.LateTick) && obj is ILateTickable lateTickable)
        _ticker.AddLate(lateTickable);

      if (flags.HasFlag(LifeCycleFlags.Dispose) && obj is IDisposable disposable)
        _disposer.Add(disposable);

      if (flags.HasFlag(LifeCycleFlags.LateDispose) && obj is ILateDisposable lateDisposable)
        _disposer.AddLate(lateDisposable);
    }

    public void Unbind(object obj, LifeCycleFlags flags = LifeCycleFlags.All)
    {
      if (flags.HasFlag(LifeCycleFlags.FixedTick) && obj is IFixedTickable fixedTickable)
        _ticker.RemoveFixed(fixedTickable);

      if (flags.HasFlag(LifeCycleFlags.Tick) && obj is ITickable tickable)
        _ticker.Remove(tickable);

      if (flags.HasFlag(LifeCycleFlags.LateTick) && obj is ILateTickable lateTickable)
        _ticker.RemoveLate(lateTickable);

      if (flags.HasFlag(LifeCycleFlags.Dispose) && obj is IDisposable disposable)
        _disposer.Remove(disposable);
    }
  }
}