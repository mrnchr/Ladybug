using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class ColliderConverter : MonoBehaviour, IEcsConverter
  {
    private Collider _collider;

    private void Awake()
    {
      _collider = GetComponentInParent<Collider>();
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Replace((ref ColliderRef colliderRef) => colliderRef.Collider = _collider);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<ColliderRef>();
    }
  }
}