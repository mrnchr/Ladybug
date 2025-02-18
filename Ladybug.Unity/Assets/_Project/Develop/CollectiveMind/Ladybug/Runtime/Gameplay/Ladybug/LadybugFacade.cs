using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
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
    public ReactiveProperty<bool> IsMoving { get; }

    public LadybugFacade(IConfigProvider configProvider, ILadybugRotator rotator)
    {
      _rotator = rotator;
      _config = configProvider.Get<LadybugConfig>();
      
      IsMoving = new ReactiveProperty<bool>(false);
    }

    public void SetVisual(LadybugVisual visual)
    {
      _visual = visual;
    }

    public void UpdateVelocity(Vector3 direction)
    {
      Velocity = direction * _config.Speed;
      IsMoving.Value = Velocity != Vector3.zero;
    }

    public void CheckBound()
    {
      _rotator.CheckBound();
    }
  }
}