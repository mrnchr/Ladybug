using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugVisual : MonoBehaviour, IEcsConverter
  {
    private static readonly int _walk = Animator.StringToHash("Walk");
    
    public EcsEntityWrapper Entity => _converter.EntityWrapper;

    [field: SerializeField]
    public LadybugSkin Skin { get; private set; }

    private IFacadeRegistry _registry;
    private LadybugConfig _config;
    private LadybugFacade _facade;
    private Rigidbody _rb;
    private Animator _animator;
    private GameObjectConverter _converter;

    [Inject]
    public void Construct(IFacadeRegistry registry, LadybugConfig config)
    {
      _registry = registry;
      _config = config;
      _facade = registry.GetFacade<LadybugFacade>();
      _rb = GetComponent<Rigidbody>();
      _animator = GetComponentInChildren<Animator>();
      _converter = GetComponent<GameObjectConverter>();
      
      _facade.Bind(this);
    }

    public void ConvertTo(EcsEntityWrapper entity)
    {
      entity.Add((ref VisualFacadeRef facadeRef) => facadeRef.Facade = _facade);
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
      entity.Del<VisualFacadeRef>();
    }

    private void Start()
    {
      Skin.Initialize();

      _facade.IsMoving.Subscribe(UpdateAnimation).AddTo(this);
      _facade.Opacity.Subscribe(ApplyOpacity).AddTo(this);
    }

    private void FixedUpdate()
    {
      _facade.CheckBounds();
      _facade.UpdateVelocity(transform.forward);

      ApplyVelocity(_facade.Velocity);
    }

    private void OnDestroy()
    {
      _registry.DisposeFacade(_facade);
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
    }
  }
}