using System;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Timing
{
  [Serializable]
  public class Timer
  {
    public float StartTime { get; set; }
    public float EndTime { get; set; }
    
    [field: SerializeField]
    public float Duration { get; set; }
    public bool Expired { get; set; }
  }
}