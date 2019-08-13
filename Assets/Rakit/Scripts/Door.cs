using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : TriggerInteract
{
  public string itemName => item.itemName;

	[Tooltip("How many items need for operate")]
	public int itemNeeded = 1;
	[Tooltip("Remove item from inventory after use")]
	public bool removeOnUse;
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


	protected override void OnPlayerEnter()
	{

		if (operate != OperateKind.Touch)
			return;

		bool o = anim.GetBool("Opened");
		if (o)
			return;

		if (itemName != "")
		{
			if (Inventory.Have(itemName) < itemNeeded)
				return;
			if (removeOnUse)
				Inventory.Remove(itemName);
		}

		StartCoroutine(OperateDoor(true));

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
	protected override void OnPlayerKey()
	{
		if (operate != OperateKind.Keyboard)
			return;

		if (working)
			return;

		bool o = anim.GetBool("Opened");

		if (!o && itemName != "")
		{
			if (Inventory.Have(itemName) < itemNeeded)
				return;
			if (removeOnUse)
				Inventory.Remove(itemName);
		}


		Debug.Log("Interact with door:" + o);
		StartCoroutine(OperateDoor(!o));
	}

}
