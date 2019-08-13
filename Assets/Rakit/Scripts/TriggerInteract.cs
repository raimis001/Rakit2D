using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum OperateKind
{
	Touch, Keyboard, Interact, Disabled
}

public class TriggerInteract : Interact
{
	public OperateKind operate;
  public InventoryItemName item;


	internal bool isPlayer;

	protected virtual void OnPlayerConact() { }
	protected virtual void OnPlayerEnter() { }
	protected virtual void OnPlayerExit() { }
	protected virtual void OnPlayerKey() { }

	private void OnTriggerStay2D(Collider2D collision)
	{
    if (operate == OperateKind.Disabled)
      return;

		if (!Player.IsPlayer(collision))
			return;

		isPlayer = true;
		OnPlayerConact();
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
    if (operate == OperateKind.Disabled)
      return;
    if (!Player.IsPlayer(collision))
			return;

		isPlayer = false;
		OnPlayerExit();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
    if (operate == OperateKind.Disabled)
      return;

    if (!Player.IsPlayer(collision))
			return;

		isPlayer = true;
		OnPlayerEnter();
	}

	private void Update()
	{
    if (operate == OperateKind.Disabled)
      return;
    if (operate != OperateKind.Keyboard)
			return;
		if (!isPlayer)
			return;

		if (Input.GetKeyDown(KeyCode.E))
		{
			OnPlayerKey();
		}
	}


}
