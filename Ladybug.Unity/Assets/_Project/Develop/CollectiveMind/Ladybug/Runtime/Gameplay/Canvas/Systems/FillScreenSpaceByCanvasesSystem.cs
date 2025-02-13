using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Utils;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Canvas
{
  public class FillScreenSpaceByCanvasesSystem : IEcsRunSystem
  {
    private readonly IEcsUniverse _universe;
    private readonly IViewFactory _viewFactory;
    private readonly EcsEntities _cameras;
    private readonly EcsEntities _canvases;
    private readonly CanvasConfig _canvasConfig;

    public FillScreenSpaceByCanvasesSystem(IEcsUniverse universe, IViewFactory viewFactory, IConfigProvider configProvider)
    {
      _universe = universe;
      _viewFactory = viewFactory;
      _canvasConfig = configProvider.Get<CanvasConfig>();

      _cameras = _universe
        .FilterGame<CameraTag>()
        .Inc<CameraData>()
        .Collect();

      _canvases = _universe
        .FilterGame<CanvasTag>()
        .Inc<ConverterRef>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper camera in _cameras)
      {
        ref CameraData cameraData = ref camera.Get<CameraData>();
        var cells = new List<Vector2Int>();
        
        Rect increasedBounds = cameraData.WorldDeepBounds;
        increasedBounds.size += Vector2.one * _canvasConfig.CanvasSize / 2;
        increasedBounds.size /= _canvasConfig.CanvasSize;
        increasedBounds.center = cameraData.WorldDeepBounds.center / _canvasConfig.CanvasSize;
        
        Vector2Int min = MathUtils.RoundToInt(increasedBounds.min);
        Vector2Int max = MathUtils.RoundToInt(increasedBounds.max) + Vector2Int.one;
        var indicesRect = new RectInt();
        indicesRect.SetMinMax(min, max);
        
        foreach (Vector2Int position in indicesRect.allPositionsWithin)
          cells.Add(position);

        foreach (EcsEntityWrapper canvas in _canvases)
        {
          Vector3 position = canvas.Get<TransformRef>().Transform.position;
          Vector2 floatIndex = new Vector2(position.x, position.z) / _canvasConfig.CanvasSize;
          Vector2Int cellIndex = MathUtils.RoundToInt(floatIndex);

          int index = cells.IndexOf(cellIndex);
          if (index != -1)
            cells.RemoveAt(index);
        }

        foreach (Vector2Int cell in cells)
        {
          var converter = _viewFactory.Create<GameObjectConverter>(EntityType.Canvas);
          converter.transform.position = new Vector3(cell.x, 0, cell.y) * _canvasConfig.CanvasSize;
          converter.transform.localScale = Vector3.one * _canvasConfig.CanvasScale;
        }
      }
    }
  }
}