using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugVisual : MonoBehaviour
  {
    private static readonly int _walk = Animator.StringToHash("Walk");
    
    private LadybugFacade _facade;
    private Rigidbody _rb;
    private Animator _animator;

    [Inject]
    public void Construct(IFacadePool pool)
    {
      _facade = pool.GetFacade<LadybugFacade>();
      _rb = GetComponent<Rigidbody>();
      _animator = GetComponentInChildren<Animator>();
      
      _facade.SetVisual(this);
    }

    private void Start()
    {
      _facade.IsMoving.Subscribe(UpdateAnimation).AddTo(this);
    }

    private void FixedUpdate()
    {
      _facade.CheckBound();
      _facade.UpdateVelocity(transform.forward);

      UpdateVelocity(_facade.Velocity);
    }

    private void UpdateVelocity(Vector3 velocity)
    {
      _rb.linearVelocity = velocity;
    }

    private void UpdateAnimation(bool isMoving)
    {
      _animator.SetBool(_walk, isMoving);
    }
  }
}