using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  public InteractLayer interactLayer;

  internal float damage;

  Rigidbody2D body;
  bool disabled;
  float direction = 1;
  void Awake()
  {
    body = GetComponent<Rigidbody2D>();
    body.isKinematic = true;
  }

  void FixedUpdate()
  {
    if (body.isKinematic)
      return;
    if (body.velocity.y < 0.1f)
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

  public static void Shot(Projectile prefab, Transform startTransform,float damage, bool isRight, float force, float dir, float destroyTime)
  {
    Projectile proj = Instantiate(prefab);

    Vector3 rot = startTransform.eulerAngles;
    rot.z = Mathf.Atan2(startTransform.right.y, startTransform.right.x) * Mathf.Rad2Deg + (isRight ? 0 : 180);

    //Debug.Log(rot);

    proj.transform.eulerAngles = rot;
    proj.transform.position = startTransform.position;

    proj.damage = damage;
    proj.Shot(force, dir, destroyTime);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (disabled)
      return;

    Interact actor = collision.GetComponentInParent<Interact>();
    if (!actor)
      return;

    if (actor.interactLayer != interactLayer)
      return;
    Debug.Log("Attacked:" + actor.gameObject.name);
    if (!actor.Attacked(2, damage))
      return;

    disabled = true;

    body.bodyType = RigidbodyType2D.Kinematic;
    body.velocity = Vector2.zero;
    body.angularVelocity = 0;

    Collider2D[] colls = GetComponentsInChildren<Collider2D>();
    foreach (Collider2D c in colls)
    {
      c.enabled = false;
    }

    transform.SetParent(actor.rangeParent ? actor.rangeParent : actor.transform);
  }
}
