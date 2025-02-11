using System;
using CollectiveMind.Ladybug.Editor.Monitoring.Component;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Sorting
{
  public interface IEcsComponentSorter
  {
    Comparison<IEcsComponentView> EcsComponentViewComparator { get; }
  }
}