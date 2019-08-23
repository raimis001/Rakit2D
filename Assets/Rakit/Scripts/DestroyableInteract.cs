﻿using UnityEngine;
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
  public override void Attack(int weapon)
  {
    if (hitPoints < 1)
      return;

    hitPoints--;
    anim.SetInteger("step",hitMax - hitPoints);
    if (hitPoints < 1)
      Destroy(gameObject);
  }
}
