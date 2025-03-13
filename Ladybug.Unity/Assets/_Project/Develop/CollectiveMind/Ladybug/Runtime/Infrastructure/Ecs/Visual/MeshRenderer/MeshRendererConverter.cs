using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class MeshRendererConverter : MonoBehaviour, IEcsConverter
  {
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
      _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref MeshRendererRef meshRendererRef) => meshRendererRef.MeshRenderer = _meshRenderer);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<MeshRendererRef>();
    }
  }
}