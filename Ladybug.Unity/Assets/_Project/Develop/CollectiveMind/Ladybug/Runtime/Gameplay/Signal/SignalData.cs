using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  [Serializable]
  public class SignalData
  {
    [LabelText("Signal Distance")]
    public float MaxDistance;

    [LabelText("Signal Colors")]
    public List<Color> Colors = new List<Color>();

    public float ColorChangingTime;
    
    [LabelText("In Camera Time")]
    public float HideDelay;
  }
}