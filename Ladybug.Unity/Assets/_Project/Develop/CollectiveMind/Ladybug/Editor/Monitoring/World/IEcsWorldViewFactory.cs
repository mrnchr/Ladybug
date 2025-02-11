using UnityEngine;

namespace CollectiveMind.Ladybug.Editor.Monitoring.World
{
  public interface IEcsWorldViewFactory
  {
    EcsWorldView Create(Transform parent);
  }
}