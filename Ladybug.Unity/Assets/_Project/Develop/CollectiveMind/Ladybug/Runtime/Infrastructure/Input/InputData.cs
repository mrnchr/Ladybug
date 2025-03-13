using System;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Input
{
  [Serializable]
  public class InputData
  {
    public bool StartDraw;
    public bool EndDraw;
    public Vector2 Position;

    public void Clear()
    {
      StartDraw = false;
      EndDraw = false;
      Position = Vector2.zero;
    }
  }
}