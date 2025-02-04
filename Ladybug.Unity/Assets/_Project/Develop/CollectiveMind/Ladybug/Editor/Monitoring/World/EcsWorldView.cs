using System.Collections.Generic;
using LudensClub.GeoChaos.Editor.Monitoring.Entity;
using UnityEngine;

namespace LudensClub.GeoChaos.Editor.Monitoring.World
{
  public class EcsWorldView : MonoBehaviour
  {
    public List<EcsEntityView> Entities = new List<EcsEntityView>();
  }
}