using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Canvas;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Input;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Line
{
  public class LineDrawer : ITickable
  {
    private static readonly int _result = Shader.PropertyToID("result");
    private static readonly int _segmentStart = Shader.PropertyToID("segment_start");
    private static readonly int _segmentEnd = Shader.PropertyToID("segment_end");
    private static readonly int _brushRadius = Shader.PropertyToID("brush_radius");
    private static readonly int _brushColor = Shader.PropertyToID("brush_color");

    private readonly InputData _inputData;
    private readonly ICanvasService _canvasSvc;
    private readonly Camera _mainCamera;
    private readonly DrawingConfig _config;
    private readonly RenderTexture _renderTexture;
    private readonly RenderTexture _whiteRenderTexture;
    private readonly int _kernelIndex;

    private Vector3 _lastPoint;
    private bool _isDrawing;

    public LineDrawer(IConfigProvider configProvider, InputData inputData, ICanvasService canvasSvc)
    {
      _inputData = inputData;
      _canvasSvc = canvasSvc;
      _mainCamera = Camera.main;

      _config = configProvider.Get<DrawingConfig>();
      _kernelIndex = _config.BrushDrawerShader.FindKernel("CSMain");

      _renderTexture = new RenderTexture(_config.TextureSize, _config.TextureSize, 24, RenderTextureFormat.ARGBFloat);
      _renderTexture.enableRandomWrite = true;
      _renderTexture.Create();

      _whiteRenderTexture =
        new RenderTexture(_config.TextureSize, _config.TextureSize, 24, RenderTextureFormat.ARGBFloat);
      _whiteRenderTexture.enableRandomWrite = true;
      _whiteRenderTexture.Create();

      Graphics.SetRenderTarget(_whiteRenderTexture);
      GL.Clear(true, true, Color.white);
      Graphics.SetRenderTarget(null);
    }

    public void Tick()
    {
      if (_inputData.Draw)
      {
        Vector3 currentPoint = Vector2.zero;
        if (Physics.Raycast(_mainCamera.ScreenPointToRay(_inputData.Position),
          out _, Mathf.Infinity, _config.CanvasLayer))
        {
          currentPoint = GetWorldCursorPoint();
          if (!_isDrawing)
          {
            _lastPoint = currentPoint;
            _isDrawing = true;
          }
          
          var start = new Vector2(_lastPoint.x, _lastPoint.z);
          var end = new Vector2(currentPoint.x, currentPoint.z);
          EcsRawEntities canvases = _canvasSvc.FindCanvasesCrossedBySegment(start, end);
          
          foreach (EcsEntityWrapper canvas in canvases)
          {
            MeshRenderer meshRenderer = canvas.Get<MeshRendererRef>().MeshRenderer;
            DrawLineOnCanvas(meshRenderer, _lastPoint, currentPoint);
          }
        }

        _lastPoint = currentPoint;
      }
      else
      {
        _isDrawing = false;
      }
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

    private Texture2D GetTexture(Renderer canvas)
    {
      Texture2D texture = (Texture2D)canvas.material.mainTexture;
      if (texture == null)
      {
        var resultTexture = new Texture2D(_config.TextureSize, _config.TextureSize, TextureFormat.RGBA32, false);
        ReadRenderTexture(_whiteRenderTexture, resultTexture);

        texture = resultTexture;
        canvas.material.mainTexture = resultTexture;
      }

      return texture;
    }

    private void DrawLine(Texture2D texture, Vector2 startPoint, Vector2 endPoint)
    {
      Graphics.Blit(texture, _renderTexture);

      _config.BrushDrawerShader.SetTexture(_kernelIndex, _result, _renderTexture);
      _config.BrushDrawerShader.SetVector(_segmentStart, startPoint);
      _config.BrushDrawerShader.SetVector(_segmentEnd, endPoint);
      _config.BrushDrawerShader.SetFloat(_brushRadius, _config.BrushRadius / _config.TextureSize);
      _config.BrushDrawerShader.SetVector(_brushColor, _config.BrushColor);

      _config.BrushDrawerShader.Dispatch(_kernelIndex, _config.TextureSize / 8, _config.TextureSize / 8, 1);

      ReadRenderTexture(_renderTexture, texture);
    }

    private void ReadRenderTexture(RenderTexture renderTexture, Texture2D texture)
    {
      RenderTexture.active = renderTexture;
      texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
      texture.Apply();
    }
  }
}