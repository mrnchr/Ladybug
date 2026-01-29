using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugRotator : ILadybugRotator
  {
    private readonly IFacadePool _facadePool;
    private readonly DrawingConfig _config;
    private readonly LadybugConfig _ladybugConfig;
    private readonly Texture2D _texture;
    private LadybugFacade _ladybugFacade;

    public LadybugRotator(IFacadePool facadePool, DrawingConfig config, LadybugConfig ladybugConfig)
    {
      _facadePool = facadePool;
      _config = config;
      _ladybugConfig = ladybugConfig;

      _texture = new Texture2D(_config.TextureSize, _config.TextureSize, _config.TextureFormat, false);
    }

    public void CheckBounds()
    {
      _ladybugFacade = _facadePool.GetFacade<LadybugFacade>();
      if (Physics.Raycast(_ladybugFacade.Transform.position, Vector3.down,
        out RaycastHit hit, Mathf.Infinity, _config.CanvasLayer))
      {
        var canvas = hit.collider.GetComponent<Renderer>();
        if (!ReadTextureFromRaw(canvas.material.mainTexture))
          return;

        Vector3 localScale = 10 * canvas.transform.localScale;
        var canvasSize = new Vector2(localScale.x, localScale.z);
        Vector3 forward = _ladybugFacade.Transform.forward;
        Vector2 right = new Vector2(_ladybugFacade.Transform.right.x, _ladybugFacade.Transform.right.z);
        Vector2 textureForward = WorldToUVDirection(canvas, forward);
        Vector2 start = hit.textureCoord;
        Vector2 end = start + textureForward * _ladybugConfig.ViewDistance / canvasSize.x;

        Vector2 pixelStart = start * _config.TextureSize;
        Vector2 pixelEnd = end * _config.TextureSize;
        float width = _ladybugConfig.ViewWidth / canvasSize.x * _config.TextureSize;
        Vector2 blackPixel = FindBlackPixel(pixelStart, pixelEnd, textureForward, width, right, _texture);
        if (blackPixel == -Vector2.one)
          return;

        Vector2 gradient = ComputeGradient(_texture, blackPixel / _config.TextureSize);
        Vector2 direction = new Vector2(-gradient.y, gradient.x).normalized;
        if (Vector2.Dot(direction, new Vector2(forward.x, forward.z)) < 0)
          direction *= -1;
        forward = new Vector3(direction.x, forward.y, direction.y);
        _ladybugFacade.Transform.forward = forward;
      }
    }

    private Vector2 WorldToUVDirection(Renderer canvas, Vector3 worldDirection)
    {
      Vector3 localMousePosition = canvas.transform.InverseTransformDirection(worldDirection);
      Vector2 localTexturePoint = new Vector2(localMousePosition.x, localMousePosition.z);

      return -localTexturePoint;
    }

    private bool ReadTextureFromRaw(Texture texture)
    {
      if (texture is not RenderTexture renderTexture)
        return false;

      RenderTexture.active = renderTexture;
      _texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
      _texture.Apply();
      return true;
    }

    private Vector2 FindBlackPixel(Vector2 pixelStart, Vector2 pixelEnd, Vector2 textureForward, float width,
      Vector2 right,
      Texture2D texture)
    {
      bool isPixelFound = false;
      for (Vector2 i = pixelStart; Vector2.Distance(i, pixelEnd) >= 1 && !isPixelFound; i += textureForward)
      {
        for (float j = -width / 2; j < width / 2 && !isPixelFound; j++)
        {
          Vector2 t = i + right * j;
          Vector2Int p = new Vector2Int(Mathf.RoundToInt(t.x), Mathf.RoundToInt(t.y));
          if (p.x >= 0 && p.x < _config.TextureSize && p.y >= 0 && p.y < _config.TextureSize
            && texture.GetPixel(p.x, p.y) != Color.white)
            return p;
        }
      }

      return new Vector2(-1, -1);
    }

    private Vector2 ComputeGradient(Texture2D texture, Vector2 uv, float windowSize = 0.03f)
    {
      float u = uv.x;
      float v = uv.y;
      int width = texture.width;
      int height = texture.height;

      float halfWindowSize = windowSize / 2;
      float uMin = Mathf.Max(0, u - halfWindowSize);
      float uMax = Mathf.Min(1, u + halfWindowSize);
      float vMin = Mathf.Max(0, v - halfWindowSize);
      float vMax = Mathf.Min(1, v + halfWindowSize);

      float gradientX = 0;
      float gradientY = 0;

      for (float du = uMin; du <= uMax; du += windowSize / 3)
      {
        for (float dv = vMin; dv <= vMax; dv += windowSize / 3)
        {
          Color left = texture.GetPixel((int)(du * width), (int)(dv * height));
          Color right = texture.GetPixel((int)((du + windowSize / 3) * width), (int)(dv * height));
          Color bottom = texture.GetPixel((int)(du * width), (int)(dv * height));
          Color top = texture.GetPixel((int)(du * width), (int)((dv + windowSize / 3) * height));

          gradientX += right.r - left.r;
          gradientY += top.r - bottom.r;
        }
      }

      return new Vector2(gradientX, gradientY);
    }
  }
}