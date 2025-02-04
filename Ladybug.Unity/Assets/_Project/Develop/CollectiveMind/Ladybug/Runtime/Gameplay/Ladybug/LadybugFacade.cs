using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugFacade : IFacade
  {
    private readonly LadybugConfig _config;

    public ReactiveProperty<Vector3> Direction { get; } = new ReactiveProperty<Vector3>();
    public ReactiveProperty<Vector3> Velocity { get; } = new ReactiveProperty<Vector3>();

    public LadybugFacade(IConfigProvider configProvider)
    {
      _config = configProvider.Get<LadybugConfig>();

      Direction.Subscribe(UpdateVelocity);
    }

    private void UpdateVelocity(Vector3 direction)
    {
      Velocity.Value = Direction.Value * _config.Speed;
    }
  }
}