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
  }
}