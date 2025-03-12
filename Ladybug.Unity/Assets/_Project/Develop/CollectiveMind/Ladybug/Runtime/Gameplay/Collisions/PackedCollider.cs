using System;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  [Serializable]
  public struct PackedCollider
  {
    public Collider Collider;

    [ShowInInspector]
    public EcsPackedEntity Entity;

    public PackedCollider(Collider collider, EcsPackedEntity entity)
    {
      Collider = collider;
      Entity = entity;
    }
  }
}