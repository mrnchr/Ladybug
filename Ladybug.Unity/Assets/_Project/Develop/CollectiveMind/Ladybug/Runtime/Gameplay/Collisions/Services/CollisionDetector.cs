using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using TriInspector;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Collisions
{
  [DeclareFoldoutGroup(nameof(CollisionDetector), Title = "Detection Conditions")]
  public class CollisionDetector : MonoBehaviour
  {
    [GroupNext(nameof(CollisionDetector))]
    [SerializeField]
    private bool WhenEnter = true;

    [SerializeField]
    private bool WhenStay;

    [SerializeField]
    private bool WhenExit;

    [SerializeField]
    private bool WhenTrigger = true;

    [SerializeField]
    private bool WhenCollider = true;

    private ICollisionFiller _filler;
    private EntityVisual _visual;
    private Collider _collider;
    private bool _hasRigidBody;

    [Inject]
    public void Construct(ICollisionFiller filler)
    {
      _filler = filler;
      _visual = GetComponentInParent<EntityVisual>(true);
      _collider = GetComponentInChildren<Collider>(true);
      _hasRigidBody = TryGetComponent<Rigidbody>(out _);
    }

    private void OnCollisionEnter(Collision collision)
    {
      if (!WhenEnter || !WhenCollider)
        return;

      SendCollision(CollisionType.Enter, collision);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!WhenEnter || !WhenTrigger)
        return;

      SendCollision(CollisionType.Enter, other);
    }

    private void OnCollisionStay(Collision collision)
    {
      if (!WhenStay || !WhenCollider)
        return;

      SendCollision(CollisionType.Stay, collision);
    }

    private void OnTriggerStay(Collider other)
    {
      if (!WhenStay || !WhenTrigger)
        return;

      SendCollision(CollisionType.Stay, other);
    }

    private void OnCollisionExit(Collision collision)
    {
      if (!WhenExit || !WhenCollider)
        return;

      SendCollision(CollisionType.Exit, collision);
    }

    private void OnTriggerExit(Collider other)
    {
      if (!WhenExit || !WhenTrigger)
        return;

      SendCollision(CollisionType.Exit, other);
    }

    private void SendCollision(CollisionType type, Collision collision)
    {
      Collider current = _collider;
      
      if (_hasRigidBody)
        current = collision.GetContact(0).thisCollider;
      
      SendCollision(type, current, collision.collider);
    }

    private void SendCollision(CollisionType type, Collider other)
    {
      SendCollision(type, _collider, other);
    }

    private void SendCollision(CollisionType type, Collider current, Collider other)
    {
      _filler.Fill(
        new OneSideCollision(type, new PackedCollider(current, _visual.Entity.PackedEntity), other));
    }
  }
}