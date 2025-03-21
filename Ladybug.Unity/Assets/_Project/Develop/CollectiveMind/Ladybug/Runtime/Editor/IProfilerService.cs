﻿#if UNITY_EDITOR
using System;

namespace CollectiveMind.Ladybug.Runtime
{
  public interface IProfilerService
  {
    string GetPrettyName(object target, string methodName, Type context);
    string GetPrettyName(object target, Type context);
    string GetPrettyName(Type type, Type context);
    string GetPrettyName(Type type);
  }
}
#endif