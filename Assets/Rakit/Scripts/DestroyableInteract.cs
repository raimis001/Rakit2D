using UnityEngine;
using System.Collections;

public class DestroyableInteract : Interact
{
  public int hitPoints = 5;

  public override void Attack(int weapon)
  {
    if (hitPoints < 1)
      return;

    hitPoints--;
    if (hitPoints < 1)
      Destroy(gameObject);
  }
}
