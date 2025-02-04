using System.Collections.Generic;
using LudensClub.GeoChaos.Editor.Monitoring.World;
using UnityEngine;

namespace LudensClub.GeoChaos.Editor.Monitoring.Universe
{
  public class EcsUniverseView : MonoBehaviour
  {
    public List<EcsWorldView> Worlds = new List<EcsWorldView>();
  }
}