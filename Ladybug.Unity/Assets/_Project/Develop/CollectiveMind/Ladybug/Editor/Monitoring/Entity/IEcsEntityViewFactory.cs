using UnityEngine;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Entity
{
  public interface IEcsEntityViewFactory
  {
    EcsEntityView Create(Transform parent);
  }
}