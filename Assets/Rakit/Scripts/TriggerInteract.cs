using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnOperateEvent : UnityEvent<Interact> { }
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

  [Header("Events")]
  public OnOperateEvent OnOperate;

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
    Debug.Log("Enter trigger");
    if (!Player.IsPlayer(collision))
      return;

    Debug.Log("Enter trigger player");

    isPlayer = true;
    OnPlayerEnter();

    if (operate != OperateKind.Touch)
      return;

    Debug.Log("Touch");

    if (!CanOperate())
      return;

    Debug.Log("Can operate");

    if (!Operate(false))
      return;

    Debug.Log("Operate");

    if (itemName != "" && removeOnUse)
      Inventory.Remove(itemName);
    if (OnOperate != null)
      OnOperate.Invoke(this);
	}

	private void Update()
	{
    if (operate != OperateKind.Keyboard)
			return;
		if (!isPlayer)
			return;
    if (!CanOperate())
      return;

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (Operate(true))
      {
        if (OnOperate != null)
          OnOperate.Invoke(this);
      }
    }
	}

  private bool CanOperate()
  {
    if (itemName != "")
    {
      if (Inventory.Have(itemName) < itemNeeded)
        return false;
    }

    return true;
  }


}
