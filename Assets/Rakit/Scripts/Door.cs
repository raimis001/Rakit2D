using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : TriggerInteract
{
  public GameObject icon;
	
	public bool closeOnExit;

	Animator anim;
	bool working;
  bool opened => anim.GetBool("Opened");
  private void Awake()
	{
		anim = GetComponent<Animator>();

    if (!opened && item.itemName != "")
      icon.SetActive(true);
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

    if (!opened && item.itemName != "")
      icon.SetActive(false);


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
