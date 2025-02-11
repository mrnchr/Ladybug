using System.Collections.Generic;
using CollectiveMind.Ladybug.Editor.Monitoring.Entity;
using UnityEngine;

namespace CollectiveMind.Ladybug.Editor.Monitoring.World
{
  public class EcsWorldView : MonoBehaviour
  {
    public List<EcsEntityView> Entities = new List<EcsEntityView>();
  }
}