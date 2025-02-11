using CollectiveMind.Ladybug.Runtime.Gameplay;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public interface IViewFactory
  {
    TComponent Create<TComponent>(EntityType id) where TComponent : Component;
  }
}