using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Input;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Line
{
  public class LineDrawer : IGameStep
  {
    private static readonly int _segmentStart = Shader.PropertyToID("_SegmentStart");
    private static readonly int _segmentEnd = Shader.PropertyToID("_SegmentEnd");
    private static readonly int _brushRadius = Shader.PropertyToID("_BrushRadius");
    private static readonly int _brushColor = Shader.PropertyToID("_BrushColor");

    private readonly InputData _inputData;
    private readonly ICanvasService _canvasSvc;
    private readonly Camera _mainCamera;
    private readonly DrawingConfig _config;
    private readonly Material _blitLineMaterial;

    private Vector3 _lastPoint;
    private bool _isDrawing;

    public LineDrawer(IConfigProvider configProvider,
      InputData inputData,
      ICanvasService canvasSvc)
    {
      _inputData = inputData;
      _canvasSvc = canvasSvc;
      _mainCamera = Camera.main;

      _config = configProvider.Get<DrawingConfig>();
      _blitLineMaterial = new Material(_config.BlitLineBrush);
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
        
        if(_inputData.StartDraw)
          _lastPoint = currentPoint;
        else if (TryFindCollision(_lastPoint, ref currentPoint))
          _isDrawing = false;

        var start = new Vector2(_lastPoint.x, _lastPoint.z);
        var end = new Vector2(currentPoint.x, currentPoint.z);
        EcsRawEntities canvases = _canvasSvc.FindCanvasesCrossedBySegment(start, end);

        foreach (EcsEntityWrapper canvas in canvases)
        {
          MeshRenderer meshRenderer = canvas.Get<MeshRendererRef>().MeshRenderer;
          DrawLineOnCanvas(meshRenderer, _lastPoint, currentPoint);
        }

        _lastPoint = currentPoint;
      }
    }

    private bool TryFindCollision(Vector3 lastPoint, ref Vector3 currentPoint)
    {
      var halfHeight = 0.5f;
      Vector3 castCenter = lastPoint + Vector3.up * halfHeight;
      Vector3 halfSize = new Vector3(0.005f, halfHeight - 0.001f, 0.005f);
      Vector3 distance = currentPoint - lastPoint;
      Vector3 direction = distance.normalized;
      if (Physics.BoxCast(castCenter, halfSize, direction, out RaycastHit hit, Quaternion.LookRotation(direction),
        distance.magnitude))
      {
        currentPoint = hit.point;
        return true;
      }
      
      return false;
    }

    private Vector3 GetWorldCursorPoint()
    {
      var deepMousePosition = new Vector3(_inputData.Position.x, _inputData.Position.y,
        Mathf.Abs(_mainCamera.transform.position.y));
      return _mainCamera.ScreenToWorldPoint(deepMousePosition);
    }

    private void DrawLineOnCanvas(Renderer canvas, Vector3 startPoint, Vector3 endPoint)
    {
      DrawLine(GetTexture(canvas), WorldToUVPoint(canvas, startPoint), WorldToUVPoint(canvas, endPoint));
    }

    private Vector2 WorldToUVPoint(Renderer canvas, Vector3 worldMousePosition)
    {
      Vector3 localMousePosition = canvas.transform.InverseTransformPoint(worldMousePosition);
      Vector2 localTexturePoint = new Vector2(localMousePosition.x, localMousePosition.z);

      Vector2 unitSize = Vector2.one * 10;
      return -(localTexturePoint - unitSize / 2) / unitSize;
    }

    private RenderTexture GetTexture(Renderer canvas)
    {
      return (RenderTexture)canvas.material.mainTexture ?? CreateTexture(canvas);
    }

    private RenderTexture CreateTexture(Renderer canvas)
    {
      var resultTexture = new RenderTexture(_config.TextureSize, _config.TextureSize, 0, _config.RenderTextureFormat);
      resultTexture.Create();
      
      Graphics.SetRenderTarget(resultTexture);
      GL.Clear(false, true, Color.white);
      Graphics.SetRenderTarget(null);
      
      canvas.material.mainTexture = resultTexture;
      return resultTexture;
    }

    private void DrawLine(RenderTexture texture, Vector2 startPoint, Vector2 endPoint)
    {
      _blitLineMaterial.SetVector(_segmentStart, new Vector4(startPoint.x, startPoint.y, 0, 0));
      _blitLineMaterial.SetVector(_segmentEnd, new Vector4(endPoint.x, endPoint.y, 0, 0));
      _blitLineMaterial.SetFloat(_brushRadius, _config.BrushRadius / _config.TextureSize);
      _blitLineMaterial.SetVector(_brushColor, _config.BrushColor);
        
      RenderTexture temp = RenderTexture.GetTemporary(texture.width, texture.height, 0, texture.format);
      
      Graphics.Blit(texture, temp, _blitLineMaterial);
      Graphics.Blit(temp, texture);
      
      RenderTexture.ReleaseTemporary(temp);
    }
  }
}