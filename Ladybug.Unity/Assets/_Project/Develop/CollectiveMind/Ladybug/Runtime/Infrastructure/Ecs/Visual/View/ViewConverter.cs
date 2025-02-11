using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public class ViewConverter : MonoBehaviour, IEcsConverter
  {
    private BaseView _view;

    private void Awake()
    {
      _view = GetComponent<BaseView>();
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref ViewRef viewRef) => viewRef.View = _view);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<ViewRef>();
    }
  }
}