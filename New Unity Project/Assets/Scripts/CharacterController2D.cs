using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] 
	private float jumpForce = 200f;
	
	[Range(0, .3f)] [SerializeField] 
	private float movementSmoothing = .05f;
	
	[SerializeField]
	private bool airControl;
	
	[SerializeField] 
	private LayerMask whatIsGround;
	
	[SerializeField] 
	private Transform groundCheck;
	
	[SerializeField] 
	private Transform ceilingCheck;

	private const float GroundedRadius = .2f; 
	private const float CeilingRadius  = .2f;
	
	private bool _grounded;
	private bool _facingRight = true;
	
	private Rigidbody2D _rigidbody2D;
	private Vector3 _velocity = Vector3.zero;
	
	public UnityEvent onLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();

		if (onLandEvent == null)
			onLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		var wasGrounded = _grounded;
		_grounded = false;

		var colliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
		foreach (var theCollider in colliders)
		{
			if (theCollider.gameObject == gameObject) continue;
			_grounded = true;
			if (!wasGrounded)
				onLandEvent.Invoke();
		}
	}


	public void Move(float move, bool jump)
	{

		if (_grounded || airControl)
		{
			var velocity = _rigidbody2D.velocity;
			Vector3 targetVelocity = new Vector2(move * 10f, velocity.y);
			_rigidbody2D.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, movementSmoothing);

			if (move > 0 && !_facingRight)
			{
				Flip();
			}
			else if (move < 0 && _facingRight)
			{
				Flip();
			}
		}

		if (!_grounded || !jump) return;
		_grounded = false;
		_rigidbody2D.AddForce(new Vector2(0f, jumpForce));
	}


	private void Flip()
	{
		_facingRight = !_facingRight;

		var transform1 = transform;
		var theScale = transform1.localScale;
		theScale.x *= -1;
		transform1.localScale = theScale;
	}

	public bool IsGrounded()
	{
		return _grounded;
	}
}