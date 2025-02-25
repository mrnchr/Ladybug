using UnityEngine;
using UnityEngine.InputSystem;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Sheep
{
  public class SheepDriver : MonoBehaviour
  {
    private static readonly int _walk = Animator.StringToHash("Walk");

    [SerializeField]
    private float _speed;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private PlayerInput _playerInput;
    private InputActionAsset _actions;

    private void Awake()
    {
      _animator = GetComponentInChildren<Animator>();
      _rigidbody = GetComponent<Rigidbody>();
      _playerInput = FindAnyObjectByType<PlayerInput>();
      _actions = _playerInput.actions;
    }

    private void Update()
    {
      var direction = _actions.FindAction("Walk").ReadValue<Vector2>();

      Vector3 vector = new Vector3(direction.x, 0, direction.y) * _speed;

      Vector3 velocity = vector;  
      velocity.y = _rigidbody.velocity.y;
      _rigidbody.velocity = velocity;

      if (vector != Vector3.zero)
        transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);

      _animator.SetBool(_walk, vector != Vector3.zero);
    }
  }
}