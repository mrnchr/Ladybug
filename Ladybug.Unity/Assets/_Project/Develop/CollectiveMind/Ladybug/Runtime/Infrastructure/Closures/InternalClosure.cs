namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Closures
{
  public abstract class InternalClosure<TType> : Closure<TType>
  {
    protected InternalClosure()
    {
      Predicate = Call;
    }

    protected abstract bool Call(TType value);
  }
}