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

  [Header("Inventory item")]
  public InventoryItemName item;
  [Tooltip("How many items need for operate")]
  public int itemNeeded = 1;
  [Tooltip("Remove item from inventory after use")]
  public bool removeOnUse;

  protected string itemName => item.itemName;
  protected bool isPlayer;

	protected virtual void OnPlayerEnter() { }
	protected virtual void OnPlayerExit() { }
  	
	private void OnTriggerExit2D(Collider2D collision)
	{
    if (!Player.IsPlayer(collision))
			return;

    isPlayer = false;
    OnPlayerExit();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
    if (!Player.IsPlayer(collision))
      return;

    isPlayer = true;
    OnPlayerEnter();

    if (operate != OperateKind.Touch)
      return;

    if (itemName != "")
    {
      if (Inventory.Have(itemName) < itemNeeded)
        return;
    }

    if (!Operate(false))
      return;

    if (itemName != "" && removeOnUse)
      Inventory.Remove(itemName);

	}

	private void Update()
	{
    if (operate != OperateKind.Keyboard)
			return;
		if (!isPlayer)
			return;
		if (Input.GetKeyDown(KeyCode.E))
		{
			Operate(true);
		}
	}


}
