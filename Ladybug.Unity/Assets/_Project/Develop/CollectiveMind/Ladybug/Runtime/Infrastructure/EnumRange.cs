using System;
using System.Runtime.CompilerServices;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure
{
  public readonly struct EnumRange<T> where T : Enum
  {
    public static readonly T[] Values = CreateValues();
    public static readonly EnumRange<T> AllRange = new EnumRange<T>(0, Values.Length);
    
    private readonly int _start;
    private readonly int _endExclusive;

    public EnumRange(T start, T endExclusive)
    {
      _start = Array.IndexOf(Values, start);

      if (_start < 0)
      {
        _start = 0;
      }

      _endExclusive = Array.IndexOf(Values, endExclusive);
      
      if (_endExclusive < 0)
      {
        _endExclusive = Values.Length;
      }
    }

    public EnumRange(int startIndex, int endIndexExclusive)
    {
      _start = Math.Clamp(startIndex, 0, Values.Length);
      _endExclusive = Math.Clamp(endIndexExclusive, 0, Values.Length);
    }

    public Enumerator GetEnumerator()
    {
      return new Enumerator(_start, _endExclusive);
    }

    private static T[] CreateValues()
    {
      Array array = Enum.GetValues(typeof(T));

      var copy = new T[array.Length];
      
      for (int i = 0; i < copy.Length; i++)
      {
        copy[i] = (T)array.GetValue(i);
      }

      return copy;
    }

    public struct Enumerator
    {
      private readonly int _end;
      private int _current;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public Enumerator(int start, int endExclusive)
      {
        _current = start - 1;
        _end = endExclusive;
      }

      public T Current
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Values[_current];
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool MoveNext()
      {
        _current++;
        
        return _current < _end;
      }
    }
  }
}