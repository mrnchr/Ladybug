using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugVisual : MonoBehaviour
  {
    private LadybugFacade _facade;
    private Rigidbody _rb;

    [Inject]
    public void Construct(IFacadePool pool)
    {
      _facade = pool.GetFacade<LadybugFacade>();
      _rb = GetComponent<Rigidbody>();
      
      _facade.SetVisual(this);
    }

    private void FixedUpdate()
    {
      _facade.CheckBound();
      _facade.UpdateVelocity(transform.forward);
      
      UpdateVelocity(_facade.Velocity);
    }

    private void UpdateVelocity(Vector3 velocity)
    {
      _rb.velocity = velocity;
    }
  }
}