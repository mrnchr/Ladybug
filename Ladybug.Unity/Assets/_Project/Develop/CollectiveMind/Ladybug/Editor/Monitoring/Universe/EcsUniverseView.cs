using System.Collections.Generic;
using CollectiveMind.Ladybug.Editor.Monitoring.World;
using UnityEngine;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Universe
{
  public class EcsUniverseView : MonoBehaviour
  {
    public List<EcsWorldView> Worlds = new List<EcsWorldView>();
  }
}