using System;
using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas
{
  public class CanvasService : ICanvasService
  {
    private readonly IEcsUniverse _universe;
    private readonly EcsEntities _canvases;
    private readonly CanvasConfig _config;

    public CanvasService(IEcsUniverse universe, IConfigProvider configProvider)
    {
      _universe = universe;
      _config = configProvider.Get<CanvasConfig>();

      _canvases = _universe
        .FilterGame<CanvasTag>()
        .Inc<ConverterRef>()
        .Collect();
    }

    public EcsEntityWrapper FindCanvas(Predicate<EcsEntityWrapper> match)
    {
      var entity = new EcsEntityWrapper();
      foreach (EcsEntityWrapper canvas in _canvases)
      {
        if (match(canvas))
        {
          entity.Copy(canvas);
          return entity;
        }
      }

      return entity;
    }

    public EcsEntityWrapper FindCanvasByTransform(Transform transform)
    {
      return FindCanvas(x => x.Get<TransformRef>().Transform == transform);
    }

    public EcsRawEntities FindNeighboursForTransform(Transform transform, Vector2 checkedAxis, Vector2 offsetDirection)
    {
      var signOffset = new Vector2(Mathf.Sign(offsetDirection.x), Mathf.Sign(offsetDirection.y));
      var position = new Vector2(transform.position.x, transform.position.z);
      Vector2 targetPosition = position + signOffset * _config.CanvasSize;

      var entities = new EcsRawEntities(_canvases.World);
      foreach (EcsEntityWrapper canvas in _canvases)
      {
        Transform otherTransform = canvas.Get<TransformRef>().Transform;
        var otherPosition = new Vector2(otherTransform.position.x, otherTransform.position.z);
        if (transform != otherTransform && HasPositionByAxis(otherPosition, targetPosition, checkedAxis))
          entities.Entities.Add(canvas.PackedEntity);
      }

      return entities;
    }

    private static bool HasPositionByAxis(Vector2 position, Vector2 target, Vector2 checkedAxis)
    {
      if (checkedAxis.x > 0 && position.x != target.x)
        return false;

      if (checkedAxis.y > 0 && position.y != target.y)
        return false;

      return true;
    }

    public EcsRawEntities FindCanvasesCrossedBySegment(Vector2 start, Vector2 end)
    {
      var entities = new EcsRawEntities(_canvases.World);
      foreach (EcsEntityWrapper canvas in _canvases)
      {
        Vector3 position = canvas.Get<TransformRef>().Transform.position;
        var position2D = new Vector2(position.x, position.z);
        var rect = new Rect(position2D, Vector2.one * _config.CanvasSize);
        rect.center = position2D;

        if (DoesSegmentIntersectRect(start, end, rect))
          entities.Entities.Add(canvas.PackedEntity);
      }

      return entities;
    }

    private bool DoesSegmentIntersectRect(Vector2 start, Vector2 end, Rect rect)
    {
      Vector2 bottomLeft = new Vector2(rect.xMin, rect.yMin);
      Vector2 bottomRight = new Vector2(rect.xMax, rect.yMin);
      Vector2 topLeft = new Vector2(rect.xMin, rect.yMax);
      Vector2 topRight = new Vector2(rect.xMax, rect.yMax);

      return DoSegmentsIntersect(start, end, bottomLeft, bottomRight)
        || DoSegmentsIntersect(start, end, bottomLeft, topLeft)
        || DoSegmentsIntersect(start, end, topLeft, topRight)
        || DoSegmentsIntersect(start, end, bottomRight, topRight)
        || IsPointInsideRect(start, rect) || IsPointInsideRect(end, rect);
    }

    private bool DoSegmentsIntersect(Vector2 start, Vector2 end, Vector2 boundStart, Vector2 boundEnd)
    {
      Vector2 r = end - start;
      Vector2 s = boundStart - start;
      Vector2 v = boundEnd - boundStart;

      float d = CrossProduct2D(r, v);
      if (d == 0)
        return CheckOverlap(start, end, boundStart, boundEnd);

      float toFirst = CrossProduct2D(s, r);
      float toSecond = CrossProduct2D(s, v);

      float u = toFirst / d;
      float t = toSecond / d;

      return u is >= 0 and <= 1 && t is >= 0 and <= 1;
    }

    private float CrossProduct2D(Vector2 v1, Vector2 v2)
    {
      return v1.x * v2.y - v1.y * v2.x;
    }

    private bool CheckOverlap(Vector2 start, Vector2 end, Vector2 boundStart, Vector2 boundEnd)
    {
      return IsPointOnSegment(start, boundStart, boundEnd) 
        || IsPointOnSegment(end, boundStart, boundEnd) 
        || IsPointOnSegment(boundStart, start, end) 
        || IsPointOnSegment(boundEnd, start, end);
    }

    private bool IsPointOnSegment(Vector2 point, Vector2 start, Vector2 end)
    {
      Vector2 r = end - start;
      Vector2 s = point - start;
      
      // Filter out points that are not on the segment and segments that are actually points.
      float cross = CrossProduct2D(r, s);
      if (cross != 0)
        return false;

      float dotProduct = Vector2.Dot(r, s);
      return dotProduct >= 0 && dotProduct <= r.sqrMagnitude;
    }

    private bool IsPointInsideRect(Vector2 point, Rect rect)
    {
      return point.x >= rect.xMin && point.x <= rect.xMax &&
        point.y >= rect.yMin && point.y <= rect.yMax;
    }
  }
}