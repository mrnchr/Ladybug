using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Creation.SpawnPoint
{
  public class SpawnPointGizmo : MonoBehaviour
  {
#if UNITY_EDITOR
    private SpawnPointVisual _visual;
    
    private void OnDrawGizmos()
    {
      if ((!_visual && !TryGetComponent(out _visual)) || _visual.SpawnedEntity == EntityType.None)
      {
        return;
      }
      
      Color color = _visual.SpawnedEntity == EntityType.Ladybug ? Color.green : Color.red;
      color.a = 0.75f;
      Gizmos.color = color;
      Gizmos.DrawSphere(transform.position, 1f);
    }
#endif
  }
}