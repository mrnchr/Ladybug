namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation
{
  public interface IEntityInitContextValue
  {
  }

  public class EntityInitContext
  {
    private IEntityInitContextValue _value;

    public void Set<T>(T value) where T : struct
    {
      _value ??= new EntityInitContext<T>();
      ((EntityInitContext<T>)_value).Set(value);
    }

    public T Get<T>() where T : struct
    {
      return ((EntityInitContext<T>)_value).Get();
    }

    public bool TryGet<T>(out T value) where T : struct
    {
      if (_value is not EntityInitContext<T> context)
      {
        value = default(T);
        return false;
      }

      value = context.Get();
      return true;
    }

    public void SetDefault<T>() where T : struct
    {
      Set(default(T));
    }
  }

  public class EntityInitContext<T> : IEntityInitContextValue where T : struct
  {
    private T _value;

    public void Set(T value)
    {
      _value = value;
    }

    public T Get()
    {
      return _value;
    }
  }
}