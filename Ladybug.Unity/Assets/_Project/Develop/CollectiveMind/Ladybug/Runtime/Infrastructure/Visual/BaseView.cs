using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  public class BaseView : MonoBehaviour
  {
    public EcsPackedEntity Entity => Converter.EntityWrapper.PackedEntity;
    
    [HideInInspector]
    public GameObjectConverter Converter;

    private void Awake()
    {
      Converter = GetComponent<GameObjectConverter>();
    }
  }
}