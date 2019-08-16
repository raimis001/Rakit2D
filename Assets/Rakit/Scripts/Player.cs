using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Range(0.5f, 10f)]
	public float speed = 1;
	[Range(0.5f, 5f)]
	public float jumpSpeed = 1;

  public Animator animator;

	public ContactFilter2D groundFilter;

	private Rigidbody2D body;
	private Collider2D groundCollider;

	public static bool IsPlayer(Collider2D collider)
	{
		return collider.GetComponent<Player>() != null;
	}

	private void Awake()
	{
		SM.player = this;
		body = GetComponent<Rigidbody2D>();
		if (!body)
		{
			Debug.LogWarning("No rigidbody for player");
		}
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		body.isKinematic = false;
	}

	private void OnDisable()
	{
		body.isKinematic = true;
	}

	private void Update()
	{

		if (SM.keyJump)
		{
			if (!isGrounded)
				return;

			body.AddForce(new Vector2(0, 5) * jumpSpeed, ForceMode2D.Impulse);
			_isGrounded = false;
      animator.SetTrigger("Jump");
			return;
		}

	}

	private void FixedUpdate()
	{
		groundChecked = false;
		Vector2 move = body.velocity;
		move.x = SM.keyMove * Time.deltaTime * speed * 100f;

    animator.SetBool("Grounded", isGrounded);
    animator.SetBool("Falling", move.y < -0.1f);

    if (Mathf.Abs(move.x) < 0.1f)
    {
      animator.SetFloat("Speed",0);
      move.x = 0;
    }
    else
    {
      bool nRight = move.x < 0;
      bool isRight = animator.GetBool("Right");
      if (nRight != isRight) {

        animator.SetBool("Right", nRight);
        animator.SetTrigger("ChangeDirection");
      }

      animator.SetFloat("Speed", 1);
    }

    body.velocity = move;

    
  }


  bool groundChecked;
	bool _isGrounded;
	bool isGrounded => groundChecked ? _isGrounded : GroundCheck();

	bool GroundCheck()
	{
		groundChecked = true;
		groundCollider = null;

		_isGrounded = body.IsTouching(groundFilter);
		if (!_isGrounded)
			return false;

		Collider2D[] colliders = new Collider2D[1];
		body.GetContacts(groundFilter, colliders);
		groundCollider = colliders[0];
		//Debug.Log("Ground - " + colliders[0].name);
		return _isGrounded;
	}
	

	public static void Ded()
	{
		Debug.Log("Player is ded");
	}
}