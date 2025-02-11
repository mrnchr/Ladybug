using UnityEditor;
using UnityEngine;

namespace CollectiveMind.Ladybug.Editor.General
{
  public static class SelectionHelper
  {
    public static bool IsSelectionOrChild(Transform check)
    {
      Transform transform = Selection.activeTransform;
      return transform && (transform == check || transform.IsChildOf(check));
    }
  }
}