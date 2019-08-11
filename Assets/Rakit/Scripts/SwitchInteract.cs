using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchInteract : TriggerInteract
{
	bool working;
	Animator anim;

	public InteractEvent OnSwitchOn;
	public InteractEvent OnSwitchOff;

	bool isOpened => anim.GetBool("Opened");

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}
	IEnumerator OperateSwitch(bool opened)
	{
		while (working)
			yield return null;

		working = true;
		anim.SetBool("Opened", opened);

		if (opened)
			OnSwitchOn.Invoke(this);
		else
			OnSwitchOff.Invoke(this);

		yield return new WaitForSeconds(1);
		working = false;
	}
	protected override void OnPlayerEnter()
	{
		if (operate != OperateKind.None)
			return;
		if (isOpened)
			return;

		StartCoroutine(OperateSwitch(true));
	}
	protected override void OnPlayerKey()
	{
		if (operate != OperateKind.Keyboard)
			return;
		if (working)
			return;

		StartCoroutine(OperateSwitch(!isOpened));

	}
}
