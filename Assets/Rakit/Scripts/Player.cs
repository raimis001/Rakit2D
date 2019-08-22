using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public static bool IsDed;
  private static Vector2 checkPoint;

  [Header("Move params")]
	[Range(0.5f, 10f)]
	public float speed = 1;
	[Range(0.5f, 5f)]
	public float jumpSpeed = 1;

  [Header("Weapons")]
  public bool isMeele;
  public bool isRange;

  [Header("Components")]
  public Animator animator;
	public ContactFilter2D groundFilter;

	private Rigidbody2D body;

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
    checkPoint = transform.position;

  }

	private void Start()
	{
    //Time.timeScale = 0.1f;
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
    if (IsDed)
    {
      if (SM.keyJump)
      {
        Respawn();
      }
      return;
    }

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
    if (IsDed)
      return;

    groundChecked = false;
		Vector2 move = body.velocity;
		move.x = SM.keyMove * Time.deltaTime * speed * 100f;

    animator.SetBool("Falling", move.y < -0.1f);
    animator.SetBool("Grounded", isGrounded);

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
		_isGrounded = body.IsTouching(groundFilter);
		return _isGrounded;
	}
	
  private bool GetGroundCollider(out Collider2D collider)
  {
    collider = null;
    if (!isGrounded)
      return false;

    Collider2D[] colliders = new Collider2D[1];
    if (body.GetContacts(groundFilter, colliders) < 1)
      return false;

    //Debug.Log("Ground - " + colliders[0].name);
    collider = colliders[0];
    return true;
  }

  public void SetDed()
  {
    if (IsDed)
      return;

    IsDed = true;
    body.isKinematic = true;
    body.velocity = Vector2.zero;
    animator.SetTrigger("Ded");

    Debug.Log("Player is ded");
  }

  public void SetSpawn(Vector2 pos)
  {
    if (!IsDed)
      return;

    IsDed = false;
    transform.position = pos;
    body.isKinematic = false;
    body.velocity = Vector2.zero;
    animator.SetTrigger("Respawn");
    animator.SetFloat("Speed", 0);

    Debug.Log("Player is respawn");
  }

  public static void Ded()
	{
    SM.player.SetDed();
	}
  public static void Respawn()
  {
    SM.player.SetSpawn(checkPoint);
  }
  public static void Save()
  {
    Debug.Log("Save position");
    checkPoint = SM.player.transform.position;
  }
}