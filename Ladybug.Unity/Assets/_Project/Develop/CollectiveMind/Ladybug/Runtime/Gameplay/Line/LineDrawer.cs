using CollectiveMind.Ladybug.Runtime.Configuration;
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

    private readonly Camera _mainCamera;
    private readonly DrawingConfig _config;
    private readonly RenderTexture _renderTexture;
    private readonly int _kernelIndex;

    private Vector2 _lastPoint;
    private Renderer _lastCanvas;
    private bool _isDrawing;

    public LineDrawer(IConfigProvider configProvider)
    {
      _mainCamera = Camera.main;

      _config = configProvider.Get<DrawingConfig>();
      _kernelIndex = _config.BrushDrawerShader.FindKernel("CSMain");

      _renderTexture = new RenderTexture(_config.TextureSize, _config.TextureSize, 24, RenderTextureFormat.ARGBFloat);
      _renderTexture.enableRandomWrite = true;
      _renderTexture.Create();

      Graphics.SetRenderTarget(_renderTexture);
      GL.Clear(true, true, Color.white);
      Graphics.SetRenderTarget(null);
    }

    public void Tick()
    {
      if (Input.GetMouseButton(0))
      {
        Vector2 currentPoint = Vector2.zero;
        if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition),
          out RaycastHit hit, Mathf.Infinity, _config.CanvasLayer))
        {
          currentPoint = hit.textureCoord;
          if (!_isDrawing)
          {
            _lastPoint = currentPoint;
            _isDrawing = true;
          }

          _lastCanvas = hit.collider.GetComponent<Renderer>();
        }
        else if (_lastCanvas)
        {
          currentPoint = GetUVCursorPoint();
        }

        if (_lastCanvas)
        {
          Texture2D texture = GetTexture();
          DrawLine(texture, currentPoint);
        }

        _lastPoint = currentPoint;
      }

      if (Input.GetMouseButtonUp(0))
      {
        _lastCanvas = null;
        _isDrawing = false;
      }
    }

    private Vector2 GetUVCursorPoint()
    {
      var deepMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
        Mathf.Abs(_mainCamera.transform.position.y - _lastCanvas.transform.position.y));
      Vector3 worldMousePosition = _mainCamera.ScreenToWorldPoint(deepMousePosition);
      Vector3 localMousePosition = _lastCanvas.transform.InverseTransformPoint(worldMousePosition);
      Vector2 localTexturePoint = new Vector2(localMousePosition.x, localMousePosition.z);

      Vector2 canvasSize = 10 * _lastCanvas.transform.localScale;
      return -(localTexturePoint - canvasSize / 2) / canvasSize;
    }

    private Texture2D GetTexture()
    {
      Texture2D texture = (Texture2D)_lastCanvas.material.mainTexture;
      if (texture == null)
      {
        var resultTexture = new Texture2D(_config.TextureSize, _config.TextureSize, TextureFormat.RGBA32, false);

        texture = resultTexture;
        _lastCanvas.material.mainTexture = resultTexture;
      }

      return texture;
    }

    private void DrawLine(Texture2D texture, Vector2 texturePoint)
    {
      _config.BrushDrawerShader.SetTexture(_kernelIndex, _result, _renderTexture);
      _config.BrushDrawerShader.SetVector(_segmentStart, _lastPoint);
      _config.BrushDrawerShader.SetVector(_segmentEnd, texturePoint);
      _config.BrushDrawerShader.SetFloat(_brushRadius, _config.BrushRadius / _config.TextureSize);
      _config.BrushDrawerShader.SetVector(_brushColor, _config.BrushColor);

      _config.BrushDrawerShader.Dispatch(_kernelIndex, _config.TextureSize / 8, _config.TextureSize / 8, 1);

      RenderTexture.active = _renderTexture;
      texture.ReadPixels(new Rect(0, 0, _config.TextureSize, _config.TextureSize), 0, 0);
      texture.Apply();
    }
  }
}