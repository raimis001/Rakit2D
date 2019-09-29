using UnityEngine;

public class PlayerMove : MonoBehaviour
{
  [Range(0.5f, 10f)]
  public float speed = 1;
  [Range(0.5f, 5f)]
  public float jumpSpeed = 1;
  public bool defaultIsRight;


  private Animator animator;
  public ContactFilter2D groundFilter;

  private Rigidbody2D body;
  bool groundChecked;
  bool _isGrounded;
  bool isGrounded => groundChecked ? _isGrounded : GroundCheck();
  public bool isRight => animator.GetBool("Right");

  private void Awake()
  {
    body = GetComponent<Rigidbody2D>();
    if (!body)
    {
      Debug.LogWarning("No rigidbody for player");
    }

    animator = GetComponentInChildren<Animator>();

    groundFilter.useLayerMask = true;
    groundFilter.useOutsideNormalAngle = true;
    groundFilter.minNormalAngle = 255;
    groundFilter.maxNormalAngle = 285;
  }

  private void OnEnable()
  {
    body.isKinematic = false;
  }

  private void OnDisable()
  {
    body.isKinematic = true;
  }
  public void StopBody()
  {
    body.isKinematic = true;
    body.velocity = Vector2.zero;

    animator.SetTrigger("Death");
  }

  public void RestoreBody()
  {
    body.isKinematic = false;
    body.velocity = Vector2.zero;

    animator.SetTrigger("Respawn");
    animator.SetFloat("Speed", 0);

  }


  private void Update()
  {
    if (SM.IsDeath)
      return;
    if (SM.dialogOpened)
      return;


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
    if (SM.IsDeath)
      return;

    groundChecked = false;
    Vector2 move = body.velocity;
    move.x = SM.keyMove * Time.deltaTime * speed * 100f;

    animator.SetBool("Falling", move.y < -0.1f);
    animator.SetBool("Grounded", isGrounded);

    if (Mathf.Abs(move.x) < 0.1f)
    {
      animator.SetFloat("Speed", 0);
      move.x = 0;
    }
    else
    {
      bool nRight = move.x < 0;
      if (nRight != isRight)
      {
        animator.SetBool("Right", nRight);
        animator.SetTrigger("ChangeDirection");
      }
      animator.SetFloat("Speed", 1);
    }

    body.velocity = move;
  }

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

    collider = colliders[0];
    return true;
  }



}
