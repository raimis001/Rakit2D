using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  Rigidbody2D body;
  bool disabled;
  float direction = 1;
  void Awake()
  {

    body = GetComponent<Rigidbody2D>();
    body.isKinematic = true;
    //Invoke("Shot", 1);
  }

  void Update()
  {
    if (body.isKinematic)
      return;

    //Debug.Log(body.velocity);
  }
  void FixedUpdate()
  {
    if (body.isKinematic)
      return;

    transform.Rotate(new Vector3(0, 0,  360.0f + direction * Vector3.Angle(transform.right, body.velocity.normalized)));
  }

  public void Shot(float force, float dir, float destroyTime)
  {
    direction = dir;
    body.isKinematic = false;
    body.AddForce(transform.right * 10 * force, ForceMode2D.Impulse);
    if (destroyTime > -1)
      Destroy(gameObject, destroyTime);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (disabled)
      return;

    Interact actor = collision.GetComponentInParent<Interact>();
    if (!actor)
      return;

    if (!actor.Attacked(2))
      return;

    disabled = true;
    Debug.Log("Arrow");
    body.isKinematic = true;
    body.velocity = Vector2.zero;
    body.angularVelocity = 0;

    transform.SetParent(actor.rangeParent ? actor.rangeParent : actor.transform);
  }
}
