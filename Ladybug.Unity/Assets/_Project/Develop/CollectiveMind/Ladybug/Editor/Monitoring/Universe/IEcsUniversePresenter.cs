using System.Collections.Generic;
using CollectiveMind.Ladybug.Editor.Monitoring.World;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Universe
{
  public interface IEcsUniversePresenter
  {
    EcsUniverseView View { get; }
    List<IEcsWorldPresenter> Children { get; }
    bool WasInitialized { get; }
  }
}