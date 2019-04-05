using UnityEngine;

[RequireComponent(typeof(Controller))]
public class PlayerMove : MonoBehaviour
{
	
	public float jumpHeight = 5;
	public float timeToJumpApex = 0.4f;
	public float moveSpeed = 20;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	
	private float _gravity;
	private float _jumpVelocity;
	private Vector3 _velocity;
	private Controller _controller;
	private float _velocityXSmoothing;
	private Animator _animator;
	private Player _player;
	
	// Animator parameters
	private static readonly int Jumping = Animator.StringToHash("Jumping");

	// Use this for initialization
	private void Start ()
	{
		_controller = GetComponent<Controller>();
		_gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		_jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
		_animator = _controller.animator;
		_player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	private void Update ()
	{
		if (_controller.collisions.Above || _controller.collisions.Below)
		{
			_velocity.y = 0;
		}

		var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (Input.GetButtonDown("Jump") && _controller.collisions.Below)
		{
			_velocity.y = _jumpVelocity;
			_animator.SetTrigger(Jumping);
		}
		
/*		if (Input.GetButtonDown("Ability1"))
		{
			// do ability
		}
		if (Input.GetButtonDown("Ability2"))
		{
			// do ability
		}
		if (Input.GetButtonDown("Escape"))
		{
			// do escape game exit or something
		}*/
		
		var targetVelocityX = input.x * moveSpeed;
		_velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller.collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		_velocity.y += Time.deltaTime * _gravity;
		_controller.Move(_velocity * Time.deltaTime);
	}
}
