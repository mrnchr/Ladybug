using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugVisual : EntityVisual, IBindable
  {
    private static readonly int _walk = Animator.StringToHash("Walk");
    
    [field: SerializeField]
    public LadybugSkin Skin { get; private set; }

    private LadybugConfig _config;
    private LadybugFacade _facade;
    private Rigidbody _rb;
    private Animator _animator;

    [Inject]
    private void Construct(LadybugConfig config)
    {
      _config = config;
      _rb = GetComponent<Rigidbody>();
      _animator = GetComponentInChildren<Animator>();
    }

    public void Bind(EcsEntityWrapper entity)
    {
      Skin.Initialize();
      
      _facade = entity.GetFacade<LadybugFacade>();
      _facade.IsMoving.Subscribe(UpdateAnimation).AddTo(this);
      _facade.Opacity.Subscribe(ApplyOpacity).AddTo(this);
    }

    private void FixedUpdate()
    {
      _facade.CheckBounds();
      _facade.UpdateVelocity(transform.forward);

      ApplyVelocity(_facade.Velocity);
    }

    private void UpdateAnimation(bool isMoving)
    {
      _animator.SetBool(_walk, isMoving);
    }

    private void ApplyOpacity(float opacity)
    {
      Skin.ApplyOpacity(_config.MinInvincibleAlpha, opacity);
    }

    private void ApplyVelocity(Vector3 velocity)
    {
      _rb.linearVelocity = velocity;
      float animationSpeed = velocity.magnitude * _config.AnimationSpeedMultiplier;
      Skin.SetWalkAnimationSpeed(animationSpeed);
    }
  }
}