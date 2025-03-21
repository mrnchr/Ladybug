﻿using System;
using System.Collections.Generic;

namespace CollectiveMind.Ladybug.Runtime.Utils
{
  public static class CSharpExtensions
  {
    public static bool AllNonAlloc<T>(this List<T> obj, Predicate<T> predicate)
    {
      foreach (T p in obj)
      {
        if (!predicate.Invoke(p))
          return false;
      }

      return true;
    }
    
    public static bool AnyNonAlloc<T>(this List<T> obj, Predicate<T> predicate)
    {
      foreach (T p in obj)
      {
        if (predicate.Invoke(p))
          return true;
      }

      return false;
    }
  }
}