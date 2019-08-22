using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TriggerInteract
{
  [Tooltip("-1 no closing trap")]
  public float closeAfter = 1;

  Animator anim;

  bool disabled;
  private void Awake()
  {
    anim = GetComponent<Animator>();
  }
  public override bool Operate(bool isKeyboard)
  {
    if (disabled)
      return false;

    anim.SetBool("active", true);
    return true;
  }
  protected override void OnPlayerExit()
  {
    if (closeAfter > -1)
      Invoke("CloseTrap", closeAfter);
  }

  private void CloseTrap()
  {
    anim.SetBool("active", false);
  }

  public void OnActivate(Interact actor)
  {
    disabled = false;

  }
  public void OnDeactivate(Interact actor)
  {
    disabled = true;
    anim.SetBool("active", false);
  }
}
