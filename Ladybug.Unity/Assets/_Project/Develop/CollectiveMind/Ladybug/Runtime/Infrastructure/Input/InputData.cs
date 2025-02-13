using System;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Input
{
  [Serializable]
  public class InputData
  {
    public bool Draw;
    public Vector2 Position;

    public void Clear()
    {
      Draw = false;
      Position = Vector2.zero;
    }
  }
}