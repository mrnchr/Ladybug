using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class GroundPointConverter : MonoBehaviour, IEcsConverter
  {
    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Replace((ref GroundPointRef groundCheckerRef) => groundCheckerRef.GroundPoint = transform);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Has<GroundPointRef>(false);
    }
  }
}