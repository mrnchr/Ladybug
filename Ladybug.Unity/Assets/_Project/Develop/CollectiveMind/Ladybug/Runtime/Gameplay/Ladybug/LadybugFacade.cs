using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugFacade : IFacade
  {
    private readonly ILadybugRotator _rotator;
    private readonly LadybugConfig _config;

    private LadybugVisual _visual;
    
    public Transform Transform => _visual.transform;
    public Vector3 Velocity { get; private set; }

    public LadybugFacade(IConfigProvider configProvider, ILadybugRotator rotator)
    {
      _rotator = rotator;
      _config = configProvider.Get<LadybugConfig>();
    }

    public void SetVisual(LadybugVisual visual)
    {
      _visual = visual;
    }

    public void UpdateVelocity(Vector3 direction)
    {
      Velocity = direction * _config.Speed;
    }

    public void CheckBound()
    {
      _rotator.CheckBound();
    }
  }
}