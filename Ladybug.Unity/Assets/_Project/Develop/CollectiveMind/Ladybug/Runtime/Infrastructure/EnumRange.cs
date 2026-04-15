using System;
using System.Collections.Generic;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure
{
  public readonly struct EnumRange<T> where T : Enum
  {
    private static readonly List<T> _allValues = CreateValues();
    private static readonly List<T> _allCopiedValues = new List<T>(_allValues);

    public static List<T> AllValues
    {
      get
      {
        _allCopiedValues.Clear();
        _allCopiedValues.AddRange(_allValues);
        return _allCopiedValues;
      }
    }

    public List<T> Values
    {
      get
      {
        _copiedValues.Clear();
        _copiedValues.AddRange(_values);
        return _copiedValues;
      }
    }
    
    private readonly List<T> _values;
    private readonly List<T> _copiedValues;
    private readonly int _start;
    private readonly int _endExclusive;

    public EnumRange(T start, T endExclusive)
    {
      _start = AllValues.IndexOf(start);

      if (_start < 0)
      {
        _start = 0;
      }

      _endExclusive = AllValues.IndexOf(endExclusive);
      
      if (_endExclusive < 0)
      {
        _endExclusive = AllValues.Count;
      }
      
      _values = AllValues.GetRange(_start, _endExclusive - _start);
      _copiedValues = new List<T>(_values);
    }

    public EnumRange(int startIndex, int endIndexExclusive)
    {
      _start = Math.Clamp(startIndex, 0, AllValues.Count);
      _endExclusive = Math.Clamp(endIndexExclusive, 0, AllValues.Count);
      _values = AllValues.GetRange(_start, _endExclusive - _start);
      _copiedValues = new List<T>(_values);
    }

    public List<T>.Enumerator GetEnumerator()
    {
      return _values.GetEnumerator();
    }

    private static List<T> CreateValues()
    {
      Array array = Enum.GetValues(typeof(T));
      var list = new List<T>(array.Length);
      
      foreach (T value in array)
      {
        list.Add(value);
      }

      return list;
    }
  }
}