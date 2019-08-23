using UnityEngine;
using System.Collections;

public class WeaponTrigger : MonoBehaviour
{
  internal float coolDown = 1;
  private bool _attacking;
  private bool _cooldown;

  internal bool attacking
  {
    get
    {
      return _attacking;
    }
    set
    {
      if (_attacking == value)
        return;

      _attacking = value;

      if (_attacking)
        Invoke("EndAttack", coolDown);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!_attacking)
      return;
    if (_cooldown)
      return;

    Interact actor = collision.GetComponentInParent<Interact>();
    if (!actor)
      return;

    _cooldown = true;
    actor.Attack(1);
  }

  void EndAttack()
  {
    _cooldown = false;
    _attacking = false;
  }
}
