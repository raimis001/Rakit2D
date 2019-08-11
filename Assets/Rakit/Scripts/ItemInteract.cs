using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : TriggerInteract
{
	public string itemName;
	public bool destroyOnGet;

	bool markAsDestroy;
	protected override void OnPlayerEnter()
	{
		if (markAsDestroy)
			return;

		if (operate != OperateKind.None)
			return;

		Operate();
	}

	protected override void OnPlayerKey()
	{
		if (markAsDestroy)
			return;
		if (operate != OperateKind.None)
			return;

		Operate();
	}
	void Operate()
	{
		if (itemName != "")
			Inventory.Add(itemName);

		if (destroyOnGet)
		{
			markAsDestroy = true;
			Destroy(gameObject, 1f);
		}

	}
}
