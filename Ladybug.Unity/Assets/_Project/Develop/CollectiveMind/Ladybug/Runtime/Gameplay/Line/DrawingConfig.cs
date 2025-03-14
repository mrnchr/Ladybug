using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Line
{
  [CreateAssetMenu(menuName = CAC.Names.DRAWING_CONFIG_MENU, fileName = CAC.Names.DRAWING_CONFIG_FILE)]
  public class DrawingConfig : ScriptableObject
  {
    public ComputeShader BrushDrawerShader;
    public RenderTextureFormat RenderTextureFormat;
    public TextureFormat TextureFormat;
    
    public LayerMask CanvasLayer;
    public float BrushRadius;
    public Color BrushColor;
    public int TextureSize;
  }
}