using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Laser
{
  public class LaserVisual : EntityVisual
  {
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private LineRenderer _lineRenderer;
    
    public Transform RayOrigin => _rayOrigin;

    private void Start()
    {
      _lineRenderer.positionCount = 1;
      _lineRenderer.SetPosition(0, _rayOrigin.position);
    }

    public void SetRayEndPosition(Vector3 position)
    {
      _lineRenderer.positionCount = 2;
      _lineRenderer.SetPosition(1, position);
    }

    public void ClearRayEndPosition()
    {
      _lineRenderer.positionCount = 1;
    }

    public void SetRayColor(Color color)
    {
      _lineRenderer.startColor = color;
      _lineRenderer.endColor = color;
    }
  }
}