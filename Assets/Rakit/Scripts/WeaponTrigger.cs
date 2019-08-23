using UnityEngine;
using System.Collections;

public class WeaponTrigger : MonoBehaviour
{
  internal float coolDown = 1;
  private bool _attacking;
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

    Interact actor = collision.GetComponent<Interact>();
    if (!actor)
      return;

    actor.Attack(1);

    Debug.Log("Weapon trigger");
  }
  void EndAttack()
  {
    _attacking = false;
  }
}
