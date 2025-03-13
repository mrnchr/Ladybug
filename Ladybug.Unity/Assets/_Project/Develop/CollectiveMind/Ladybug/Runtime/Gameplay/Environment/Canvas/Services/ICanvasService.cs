using System;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas
{
  public interface ICanvasService
  {
    EcsEntityWrapper FindCanvas(Predicate<EcsEntityWrapper> match);
    EcsEntityWrapper FindCanvasByTransform(Transform transform);
    EcsRawEntities FindNeighboursForTransform(Transform transform, Vector2 checkedAxis, Vector2 offsetDirection);
    EcsRawEntities FindCanvasesCrossedBySegment(Vector2 start, Vector2 end);
  }
}