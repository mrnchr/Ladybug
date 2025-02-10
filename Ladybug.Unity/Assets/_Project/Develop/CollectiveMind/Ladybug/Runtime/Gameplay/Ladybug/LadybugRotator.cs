using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Line;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugRotator : ILadybugRotator
  {
    private LadybugFacade _ladybugFacade;
    private readonly DrawingConfig _config;
    private readonly IFacadePool _facadePool;
    private readonly LadybugConfig _ladybugConfig;

    public LadybugRotator(IFacadePool facadePool, IConfigProvider configProvider)
    {
      _facadePool = facadePool;
      _config = configProvider.Get<DrawingConfig>();
      _ladybugConfig = configProvider.Get<LadybugConfig>();
    }

    public void CheckBound()
    {
      _ladybugFacade = _facadePool.GetFacade<LadybugFacade>();
      if (Physics.Raycast(_ladybugFacade.Transform.position, Vector3.down,
        out RaycastHit hit, Mathf.Infinity, _config.CanvasLayer))
      {
        var canvas = hit.collider.GetComponent<Renderer>();
        var texture = (Texture2D)canvas.material.mainTexture;
        if (!texture)
          return;

        Vector3 localScale = 10 * canvas.transform.localScale;
        var canvasSize = new Vector2(localScale.x, localScale.z);
        Vector2 right = new Vector2(_ladybugFacade.Transform.right.x, _ladybugFacade.Transform.right.z);
        Vector2 textureForward = WorldToUVDirection(canvas, _ladybugFacade.Transform.forward);
        Vector2 start = hit.textureCoord;
        Vector2 end = start + textureForward * _ladybugConfig.ViewDistance / canvasSize.x;

        Vector2 pixelStart = start * _config.TextureSize;
        Vector2 pixelEnd = end * _config.TextureSize;
        float width = _ladybugConfig.ViewWidth / canvasSize.x * _config.TextureSize;
        // Debug.Log(
          // $"view: {width}, right: {right}, textureForward: {textureForward}, start: {start}, end: {end}, pixelStart: {pixelStart}, pixelEnd: {pixelEnd}");
        for (Vector2 i = pixelStart; Vector2.Distance(i, pixelEnd) >= 1; i += textureForward)
        {
          for (float j = - width / 2; j < width / 2; j++)
          {
            Vector2 t = i + right * j;
            Vector2Int p = new Vector2Int(Mathf.RoundToInt(t.x), Mathf.RoundToInt(t.y));
            // Debug.Log($"i: {i}, j: {j}, t: {t}, p: {p}, Distance: {Vector2.Distance(i, pixelEnd)}");
            if (p.x >= 0 && p.x < _config.TextureSize && p.y >= 0 && p.y < _config.TextureSize)
              texture.SetPixel(p.x, p.y, Color.red);
          }
        }

        texture.Apply();
      }
    }

    private Vector2 WorldToUVDirection(Renderer canvas, Vector3 worldDirection)
    {
      Vector3 localMousePosition = canvas.transform.InverseTransformDirection(worldDirection);
      Vector2 localTexturePoint = new Vector2(localMousePosition.x, localMousePosition.z);

      return -localTexturePoint;
    }
  }
}