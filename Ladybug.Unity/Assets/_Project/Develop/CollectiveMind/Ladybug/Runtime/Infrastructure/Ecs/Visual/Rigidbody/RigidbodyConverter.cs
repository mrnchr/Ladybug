using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class RigidbodyConverter : MonoBehaviour, IEcsConverter
  {
    private Rigidbody _rigidbody;

    private void Awake()
    {
      _rigidbody = GetComponentInParent<Rigidbody>();
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Replace((ref RigidbodyRef rigidbodyRef) => rigidbodyRef.Rigidbody = _rigidbody);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<RigidbodyRef>();
    }
  }
}