using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Utils
{
  public static class MathUtils
  {
    public static Vector2 Round(Vector2 v)
    {
      return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static Vector2Int RoundToInt(Vector2 v)
    {
      return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static float DivideOrZero(float dividend, float divisor)
    {
      return divisor != 0 ? dividend / divisor : 0;
    }

    public static Vector2 FindClosestPointOnRect(Rect rect, Vector2 position)
    {
      Vector2 closestPoint = Vector2.zero;

      for (int i = 0; i < 2; i++)
      {
        if (position[i] < rect.min[i])
        {
          closestPoint[i] = rect.min[i];
        }
        else if (position[i] > rect.max[i])
        {
          closestPoint[i] = rect.max[i];
        }
        else
        {
          closestPoint[i] = position[i];
        }
      }

      return closestPoint;
    }

    public static float DistanceFromPointToRect(Rect rect, Vector2 position)
    {
      Vector2 closestPoint = FindClosestPointOnRect(rect, position);
      return Vector2.Distance(position, closestPoint);
    }
  }
}