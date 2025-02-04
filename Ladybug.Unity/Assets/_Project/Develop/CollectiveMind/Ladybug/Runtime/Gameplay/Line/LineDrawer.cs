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

    private Vector2 _lastPosition;

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
      if (Input.GetMouseButton(0) && Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition),
        out RaycastHit hit, Mathf.Infinity, _config.CanvasLayer))
      {
        Vector2 screenPoint = hit.textureCoord;
        if (Input.GetMouseButtonDown(0))
          _lastPosition = screenPoint;

        var renderer = hit.collider.GetComponent<Renderer>();
        Texture2D texture = (Texture2D)renderer.material.mainTexture;
        if (texture == null)
        {
          var resultTexture = new Texture2D(_config.TextureSize, _config.TextureSize, TextureFormat.RGBA32, false);

          texture = resultTexture;
          renderer.material.mainTexture = resultTexture;
        }

        _config.BrushDrawerShader.SetTexture(_kernelIndex, _result, _renderTexture);
        _config.BrushDrawerShader.SetVector(_segmentStart, _lastPosition);
        _config.BrushDrawerShader.SetVector(_segmentEnd, screenPoint);
        _config.BrushDrawerShader.SetFloat(_brushRadius, _config.BrushRadius / _config.TextureSize);
        _config.BrushDrawerShader.SetVector(_brushColor, _config.BrushColor);

        _config.BrushDrawerShader.Dispatch(_kernelIndex, _config.TextureSize / 8, _config.TextureSize / 8, 1);

        RenderTexture.active = _renderTexture;
        texture.ReadPixels(new Rect(0, 0, _config.TextureSize, _config.TextureSize), 0, 0);
        texture.Apply();

        _lastPosition = screenPoint;
      }
    }
  }
}