using System;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Input;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Line
{
  public class LineDrawer : IGameStep, IDisposable
  {
    private const float CANVAS_WORLD_SIZE = 10;
    
    private static readonly int _segmentStart = Shader.PropertyToID("_SegmentStart");
    private static readonly int _segmentEnd = Shader.PropertyToID("_SegmentEnd");
    private static readonly int _brushRadius = Shader.PropertyToID("_BrushRadius");
    private static readonly int _brushColor = Shader.PropertyToID("_BrushColor");

    private readonly DrawingConfig _config;
    private readonly InputData _inputData;
    private readonly ICanvasService _canvasService;
    private readonly Camera _mainCamera;
    private readonly Material _blitLineMaterial;

    private Vector3 _lastPoint;
    private bool _isDrawing;

    public LineDrawer(DrawingConfig config,
      InputData inputData,
      ICanvasService canvasService)
    {
      _config = config;
      _inputData = inputData;
      _canvasService = canvasService;
      _mainCamera = Camera.main;

      _blitLineMaterial = new Material(_config.BlitLineBrush);
    }

    public void Dispose()
    {
      Object.Destroy(_blitLineMaterial);
    }

    public void StopDrawing()
    {
      _isDrawing = false;
    }

    public void DrawLine(Vector3 startPoint, Vector3 endPoint, float width, Color color)
    {
      float uvSize = width / CANVAS_WORLD_SIZE;
      DrawLineInternal(startPoint, endPoint, uvSize, color);
    }

    public void Step()
    {
      if (_inputData.StartDraw)
        _isDrawing = true;

      if (_inputData.EndDraw)
        _isDrawing = false;

      if (_isDrawing)
      {
        Vector3 currentPoint = GetWorldCursorPoint();

        if (_inputData.StartDraw)
          _lastPoint = currentPoint;
        else if (TryClampSegmentByCollision(_lastPoint, ref currentPoint))
          _isDrawing = false;

        float uvSize = _config.BrushRadius / _config.TextureSize;
        DrawLineInternal(_lastPoint, currentPoint, uvSize, _config.BrushColor);
        _lastPoint = currentPoint;
      }
    }

    private Vector3 GetWorldCursorPoint()
    {
      var depthMousePosition = new Vector3(_inputData.Position.x, _inputData.Position.y,
        Mathf.Abs(_mainCamera.transform.position.y));
      return _mainCamera.ScreenToWorldPoint(depthMousePosition);
    }

    private bool TryClampSegmentByCollision(Vector3 lastPoint, ref Vector3 currentPoint)
    {
      var halfHeight = 0.5f;
      Vector3 castCenter = lastPoint + Vector3.up * halfHeight;
      Vector3 halfSize = new Vector3(0.005f, halfHeight - 0.001f, 0.005f);
      Vector3 distance = currentPoint - lastPoint;
      Vector3 direction = distance.normalized;
      Quaternion orientation = direction != Vector3.zero ? Quaternion.LookRotation(direction) : Quaternion.identity;

      if (Physics.BoxCast(castCenter, halfSize, direction, out RaycastHit hit, orientation, distance.magnitude))
      {
        currentPoint = hit.point;
        return true;
      }

      return false;
    }

    private void DrawLineInternal(Vector3 startPoint, Vector3 endPoint, float uvSize, Color color)
    {
      var start = new Vector2(startPoint.x, startPoint.z);
      var end = new Vector2(endPoint.x, endPoint.z);
      EcsRawEntities canvases = _canvasService.FindCanvasesCrossedBySegment(start, end);

      foreach (EcsEntityWrapper canvas in canvases)
      {
        MeshRenderer meshRenderer = canvas.Get<MeshRendererRef>().MeshRenderer;
        DrawLineOnCanvas(meshRenderer, startPoint, endPoint, uvSize, color);
      }
    }

    private void DrawLineOnCanvas(Renderer canvas, Vector3 startPoint, Vector3 endPoint, float uvSize, Color color)
    {
      RenderTexture texture = GetTexture(canvas);
      Vector2 start = WorldToUVPoint(canvas, startPoint);
      Vector2 end = WorldToUVPoint(canvas, endPoint);
      DrawLine(texture, start, end, uvSize, color);
    }

    private RenderTexture GetTexture(Renderer canvasRenderer)
    {
      return (RenderTexture)canvasRenderer.material.mainTexture ?? CreateTexture(canvasRenderer);
    }

    private RenderTexture CreateTexture(Renderer canvasRenderer)
    {
      var resultTexture = new RenderTexture(_config.TextureSize, _config.TextureSize, 0, _config.RenderTextureFormat);
      resultTexture.Create();

      Graphics.SetRenderTarget(resultTexture);
      GL.Clear(false, true, Color.white);
      Graphics.SetRenderTarget(null);

      canvasRenderer.material.mainTexture = resultTexture;
      return resultTexture;
    }

    private Vector2 WorldToUVPoint(Renderer canvas, Vector3 worldMousePosition)
    {
      Vector3 localMousePosition = canvas.transform.InverseTransformPoint(worldMousePosition);
      Vector2 localTexturePoint = new Vector2(localMousePosition.x, localMousePosition.z);

      Vector2 size = Vector2.one * CANVAS_WORLD_SIZE;
      return -(localTexturePoint - size / 2) / size;
    }

    private void DrawLine(RenderTexture texture, Vector2 startPoint, Vector2 endPoint, float uvSize, Color color)
    {
      _blitLineMaterial.SetVector(_segmentStart, new Vector4(startPoint.x, startPoint.y, 0, 0));
      _blitLineMaterial.SetVector(_segmentEnd, new Vector4(endPoint.x, endPoint.y, 0, 0));
      _blitLineMaterial.SetFloat(_brushRadius, uvSize);
      _blitLineMaterial.SetVector(_brushColor, color);

      RenderTexture temp = RenderTexture.GetTemporary(texture.width, texture.height, 0, texture.format);

      Graphics.Blit(texture, temp, _blitLineMaterial);
      Graphics.Blit(temp, texture);

      RenderTexture.ReleaseTemporary(temp);
    }
  }
}