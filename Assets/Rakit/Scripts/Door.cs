using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : TriggerInteract
{

	
	public bool closeOnExit;

	Animator anim;
	bool working;
	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void OpenDoor(Interact i)
	{
		StartCoroutine(OperateDoor(true));
	}
	public void CloseDoor(Interact i)
	{
		StartCoroutine(OperateDoor(false));
	}

	IEnumerator OperateDoor(bool opened)
	{
		while (working)
			yield return null;

		working = true;
		anim.SetBool("Opened", opened);

		yield return new WaitForSeconds(1);
		working = false;
	}

	protected override void OnPlayerExit()
	{
		if (!closeOnExit)
			return;

    bool o = anim.GetBool("Opened");
		if (!o)
			return;

		StartCoroutine(OperateDoor(false));
	}
  public override bool Operate(bool isKeyboard)
  {
    if (working)
      return false;

    bool opened = anim.GetBool("Opened");

    if (isKeyboard)
    {
      StartCoroutine(OperateDoor(!opened));
      return true;
    }
    if (opened)
      return false;

    StartCoroutine(OperateDoor(true));
    return true;
  }


}
