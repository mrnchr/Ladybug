using CollectiveMind.Ladybug.Runtime.Gameplay;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public class ViewFactory : IViewFactory
  {
    private readonly IInstantiator _instantiator;
    private readonly PrefabsProvider _prefabsProvider;

    public ViewFactory(IInstantiator instantiator, PrefabsProvider prefabsProvider)
    {
      _instantiator = instantiator;
      _prefabsProvider = prefabsProvider;
    }
    
    public TComponent Create<TComponent>(EntityType id) where TComponent : Component
    {
      return _instantiator.InstantiatePrefabForComponent<TComponent>(_prefabsProvider.Get(id));
    }
  }
}