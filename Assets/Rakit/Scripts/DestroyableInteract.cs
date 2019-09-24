using UnityEngine;
using System.Collections;

public class DestroyableInteract : Interact
{
  public int hitPoints = 5;
  private Animator anim;
  private int hitMax;
  private void Awake()
  {
    hitMax = hitPoints;
    anim = GetComponent<Animator>();
  }
  public override bool Attacked(int weapon, float damage = 0)
  {
    if (hitPoints < 1)
      return false;

    hitPoints--;
    anim.SetInteger("step",hitMax - hitPoints);
    if (hitPoints < 1)
      Destroy(gameObject);

    return true;
  }
}
