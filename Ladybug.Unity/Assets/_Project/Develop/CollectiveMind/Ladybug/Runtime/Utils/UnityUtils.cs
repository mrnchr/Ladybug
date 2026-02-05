using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Utils
{
  public static class UnityUtils
  {
    public static void SetPosition(this Transform transform, Axis axis, float value)
    {
      if (axis == Axis.None)
      {
        return;
      }

      Vector3 position = transform.position;
      position[(int)axis - 1] = value;
      transform.position = position;
    }

    public static void SetPosition(this Transform transform, Axis axis1, float value1, Axis axis2, float value2)
    {
      if (axis1 == Axis.None || axis2 == Axis.None)
      {
        return;
      }
      
      Vector3 position = transform.position;
      position[(int)axis1 - 1] = value1;
      position[(int)axis2 - 1] = value2;
      transform.position = position;
    }
  }

  public enum Axis
  {
    None = 0,
    X = 1,
    Y = 2,
    Z = 3
  }
}